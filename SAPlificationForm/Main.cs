using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

/* SAPlify - David Maloney - 18/08/2021
 * Program Flow 
 *  -> Execute VBA script which opens SAP 
 *  -> Open Refresh Outlook 
 *  -> Download attachments (.txt) with subject "SAP Job:"
 *  -> Read .txt attachment 
 *  -> write into excel spread sheet
 *  -> execute VBA macro to publish data in SAP
 *  -> Clean up
 */


namespace SAPlificationForm
{
    public partial class SAPlification : Form
    {
        private Thread thread;
        private bool mouseDown;
        private bool threadLock;
        private Point lastLocation;
        private string password;
        private string user;
        private string errorLogPath = Directory.GetCurrentDirectory() + @"\" + "Error Log" + @"\" + "Error Log.txt";
        private int counter = 0;
        private int counterMins = 0;
        private int jobsRaised = 0;
        private int jobsCounter = 0;
        private bool pressed = false;
        bool sapOpen = false;
        private System.Windows.Forms.Timer tm1;
        delegate void SetTextCountDownCallback(string text);
        delegate void SetTextJobsRaisedCallback(string text);
        [DllImport("user32")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        private const byte VK_MENU = 0x12;
        private const byte VK_TAB = 0x09;
        private const int KEYEVENTF_EXTENDEDKEY = 0x01;
        private const int KEYEVENTF_KEYUP = 0x02;
        private SAPJobDetails prevJob;

        // This number has to be set first run. 
        private int sharePointIndex;
        private SharePointAPI sharePointConnection;

        public SAPlification(string userName, string password)
        {
            this.user = userName;
            this.password = password;
            InitializeComponent();
            lblStatus.Text = "Waiting for user Input";
            this.Icon = Properties.Resources.logonotitle;
            counterMins = 20;
            PowerHelper.ForceSystemAwake();
        }
    
        // creates a border -> needs this for a windowless form border
        private void Main_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.Black, 1), 0, 0, 0, this.Height);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), 0, 0, this.Width, 0);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), 0, this.Height - 1, this.Width, this.Height - 1);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), this.Width - 1, 0, this.Width - 1, this.Height);
        }

        // Allows the user to move the window by clicking and dragging -> functionally same as the menu bar move
        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        // Allows the user to move the window by clicking and dragging -> functionally same as the menu bar move
        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        // Allows the user to move the window by clicking and dragging -> functionally same as the menu bar move
        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        // Start button
        private void btnOpenSAP_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Please open SAP and press Start";
            if (pressed)
            {
                btnOpenSAP.Text = "Start";
                pressed = false;
                CleanUp();
                KillSAP();
            }
            else
            {
                btnOpenSAP.Text = "Stop";
                pressed = true;
                StartProcess();
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Waiting for SAP to open...";

            SAPLogin();

            lblStatus.Text = "SAP has been opened, press Start to log work";
        }

        // presses the alt + tab key on the keyboard
        private void AltTab()
        {
            keybd_event(VK_MENU, 0, 0, 0);
            keybd_event(VK_TAB, 0, 0, 0);
            System.Threading.Thread.Sleep(1000);
            keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, 0);
        }

        // Master controller for starting methods
        private void StartProcess()
        {
            jobsCounter = 0;
            // Add check for SAP process open
            var sapProcess = Process.GetProcesses().Where(pr => pr.ProcessName == "saplogon");
            if (sapProcess.Any())
            {
                // get the sharepoint job index number as a starting point. 
                UpdateXML uxml = new UpdateXML(Directory.GetCurrentDirectory() + "\\JobLog.xml");
                sharePointIndex = uxml.GetLastJobID();

                lblStatus.Text = "Accessing SharePoint Database...";

                // Operate the thinking part of the program in a new thread - if not locks the GUI application. 
                thread = new Thread(() => SharePointAccess());
                thread.Start();
                thread.Join();

                
                thread = new Thread(() => SharePointUpdate());
                thread.Start();

                lblStatus.Text = "Process Running";
            }
            else
            {
                lblStatus.Text = "Didn't detect an open instance of SAP";
                btnOpenSAP.Text = "Start";
                pressed = false;
            }                        
        }

        private void SharePointAccess()
        {
            try
            {
                sharePointConnection = new SharePointAPI();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SharePointAccess() has failed.");
                MessageBox.Show(ex.Message);
            }
            
        }
        private void SetTextJobsRaised(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblCountDown.InvokeRequired)
            {
                SetTextJobsRaisedCallback d = new SetTextJobsRaisedCallback(SetTextJobsRaised);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblJobsRaised.Text = "Jobs processed this session: " + text;
            }
        }

        // Kill excel left open
        private void CleanUp()
        {
            // Finds process with the name EXCEL and kills it
            var excelProcess = Process.GetProcesses().Where(pr => pr.ProcessName == "EXCEL");
            foreach (var process in excelProcess)
            {
                process.Kill();
            }            
        }

        // Kill SAP left open
        private void KillSAP()
        {
            string filePath = Directory.GetCurrentDirectory() + @"\" + "Error Log" + @"\" + "Error Log.txt";

            // Throws a exception when run - looks to be due to admin rights - the try catch prevents program crashing. 
            try
            {
                // Finds process with the name saplogon and kills it
                var sapProcess = Process.GetProcesses().Where(pr => pr.ProcessName == "saplogon");
                foreach (var process in sapProcess)
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                        
        }
        
        // Uses a VBA macro to login to SAP
        private void SAPLogin()
        {
            try
            {
                lblStatus.Text = "Opening SAP";
                // Define Excel Objects
                Excel.Application xlApp = new Excel.Application();

                Excel.Workbook xlWorkBook;

                var curDir = Directory.GetCurrentDirectory() + "\\autologin.xlsm";

                // Start Excel and open the workbook.
                xlWorkBook = xlApp.Workbooks.Open(curDir);

                Excel.Worksheet sheet = (Excel.Worksheet)xlWorkBook.ActiveSheet;

                // write user and password to Excel workbook.
                sheet.Cells[1, 1].Value = user;
                sheet.Cells[1, 2].Value = password;

                // Run the macro in the spread sheet by supplying the necessary arguments
                xlApp.Run("login");
                AltTab();
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("%{O}");

                // Clean-up: Close the workbook
                xlWorkBook.Close(false);

                // Quit the Excel Application
                xlApp.Quit();

                // Clean Up
                releaseObject(xlApp);
                releaseObject(xlWorkBook);

                Thread.Sleep(5000);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }            
        }
        
        // Excel Clean up - doesnt work real well - added a process.kill to fix
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void SharePointUpdate()
        {
            jobsCounter++;
            if (jobsCounter < 100)
            {
                threadLock = true;
                string fileName = Directory.GetCurrentDirectory() + "\\JobLog.xml";

                UpdateXML uxml = new UpdateXML(fileName);
                SAPJobDetails sjd;
                try
                {
                    sjd = sharePointConnection.AccessListData(sharePointIndex);
                }
                catch (Exception ex)
                {
                    sjd = null;
                    MessageBox.Show("SharePointAccess() has failed.");
                    MessageBox.Show(ex.Message);
                }

                if (sjd != null)
                {
                    try
                    {
                        uxml.UpdateXMLFile(sjd);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("UpdateXMLFile() has failed.");
                        MessageBox.Show(e.Message);
                    }

                    try
                    {
                        if (prevJob != null)
                        {
                            if (prevJob.Time != sjd.Title && prevJob.SAPAreaCode != sjd.SAPAreaCode && prevJob.ShortDesc != sjd.ShortDesc && prevJob.Time != sjd.Time)
                            {
                                RaiseJob(sjd);
                            }
                        }
                        else
                        {
                            RaiseJob(sjd);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("RaiseJob() has failed.");
                        MessageBox.Show(ex.Message);
                    }

                    try
                    {
                        prevJob = sjd;
                        sharePointIndex = sjd.ID + 1;
                        SharePointUpdate();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Call-back SharePointUpdate() has failed.");
                        MessageBox.Show(e.Message);
                    }

                }
                threadLock = false;
            }
        }        

        // Write job details to excel -> use vba macro to upload to SAP
        private void RaiseJob(SAPJobDetails jobDetail)
        {
            try
            {
                Excel.Application xlApp = new Excel.Application();

                Excel.Workbook xlWorkBook;

                var curDir = Directory.GetCurrentDirectory() + "\\CreateJob.xlsm";

                //~~> Start Excel and open the workbook.
                xlWorkBook = xlApp.Workbooks.Open(curDir);

                Excel.Worksheet sheet = (Excel.Worksheet)xlWorkBook.ActiveSheet;

                // Writes Downloaded data into Excel                
                sheet.Cells[2, 1].Value = jobDetail.Title;
                sheet.Cells[2, 2].Value = jobDetail.SAPAreaCode;
                sheet.Cells[2, 3].Value = jobDetail.ShortDesc;
                sheet.Cells[2, 4].Value = jobDetail.LongDesc;
                sheet.Cells[2, 5].Value = jobDetail.Time;
                if(!jobDetail.AdditionalWorkerOne.Equals("n/a"))
                {
                    if(!jobDetail.AdditionalWorkerOne.Equals(jobDetail.Title))
                    {
                        sheet.Cells[2, 6].Value = jobDetail.AdditionalWorkerOne;
                    }                    
                }
                if (!jobDetail.AdditionalWorkerTwo.Equals("n/a"))
                {
                    sheet.Cells[2, 7].Value = jobDetail.AdditionalWorkerTwo;
                }                    
                sheet.Cells[2, 8].Value = jobDetail.WorkType;
                sheet.Cells[2, 9].Value = jobDetail.PartNumOne;
                sheet.Cells[2, 10].Value = jobDetail.PartQtyOne;
                sheet.Cells[2, 11].Value = jobDetail.JobType;
                sheet.Cells[2, 12].Value = jobDetail.WorkCenter;
                sheet.Cells[2, 13].Value = jobDetail.Priority;
                sheet.Cells[2, 14].Value = jobDetail.TypeZ2;
                sheet.Cells[2, 15].Value = jobDetail.JseaID;
                sheet.Cells[2, 16].Value = jobDetail.PartNumTwo;
                sheet.Cells[2, 17].Value = jobDetail.PartQtyTwo;

                // Run SAP Macro
                xlApp.Run("MasterController");

                xlWorkBook.Close(false);

                //~~> Quit the Excel Application
                xlApp.Quit();

                //~~> Clean Up
                releaseObject(xlApp);
                releaseObject(xlWorkBook);

                jobsRaised++;

                SetTextJobsRaised(jobsRaised.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // Exit button
        private void button1_Click(object sender, EventArgs e)
        {
            CleanUp();
            KillSAP();
            Application.Exit();
        }

        private void SAPlification_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        
    }
}
