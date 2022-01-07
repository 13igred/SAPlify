using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using saprotwr;
using sapfewse;

namespace SAPlificationForm
{
    class SAPScripting
    {
        SAPJobDetails jobDetails;
        public SAPScripting(SAPJobDetails jobDetail)
        {
            this.jobDetails = jobDetail;

            //Get the Windows Running Object Table
            saprotwr.net.CSapROTWrapper sapROTWrapper = new saprotwr.net.CSapROTWrapper();
            //Get the ROT Entry for the SAP Gui to connect to the COM
            object SapGuilRot = sapROTWrapper.GetROTEntry("SAPGUI");
            //Get the reference to the Scripting Engine
            object engine = SapGuilRot.GetType().InvokeMember("GetScriptingEngine", System.Reflection.BindingFlags.InvokeMethod, null, SapGuilRot, null);
            //Get the reference to the running SAP Application Window
            GuiApplication GuiApp = (GuiApplication)engine;
            //Get the reference to the first open connection
            GuiConnection connection = (GuiConnection)GuiApp.Connections.ElementAt(0);
            //get the first available session
            GuiSession session = (GuiSession)connection.Children.ElementAt(0);
            //Get the reference to the main "Frame" in which to send virtual key commands
            GuiFrameWindow frame = (GuiFrameWindow)session.FindById("wnd[0]");

            CreateZ1(session, frame);
        }

        // Raise a Z1 and add time completion
        private void CreateZ1(GuiSession session, GuiFrameWindow frame)
        {
            GuiButton btn = (GuiButton)session.FindById("wnd[0]/tbar[0]/btn[12]");
            btn.SetFocus();
            btn.Press();

            btn = (GuiButton)session.FindById("wnd[0]/tbar[0]/btn[15]");
            btn.SetFocus();
            btn.Press();

            btn = (GuiButton)session.FindById("wnd[1]/usr/btnSPOP-OPTION2");
            btn.SetFocus();
            btn.Press();

            GuiTextField menuText = (GuiTextField)session.FindById("wnd[0]/tbar[0]/okcd");
            menuText.Text = "/niw21";
            
            frame.SendVKey(0);

            menuText = (GuiTextField)session.FindById("wnd[0]/usr/ctxtRIWO00-QMART");
            menuText.Text = "z1";

            GuiTextField temp = (GuiTextField)session.FindById("wnd[0]/usr/ctxtRIWO00-QMART");
            temp.CaretPosition = 2;

            btn = (GuiButton)session.FindById("wnd[0]/tbar[0]/btn[0]");
            btn.SetFocus();
            btn.Press();

            GuiTextField SAPArea = (GuiTextField)session.FindById("wnd[0]/usr/tabsTAB_GROUP_10/tabp10\\TAB01/ssubSUB_GROUP_10:SAPLIQS0:7235/subCUSTOM_SCREEN:SAPLIQS0:7212/subSUBSCREEN_1:SAPLIQS0:7322/subOBJEKT:SAPLIWO1:0100/ctxtRIWO1-TPLNR");     //Func Loc
            SAPArea.Text = jobDetails.SAPAreaCode;

            GuiButton cbSelect = (GuiButton)session.FindById("wnd[0]/usr/tabsTAB_GROUP_10/tabp10\\TAB01/ssubSUB_GROUP_10:SAPLIQS0:7235/subCUSTOM_SCREEN:SAPLIQS0:7212/subSUBSCREEN_3:SAPLIQS0:7328/chkVIQMEL-MSAUS");        // For the breakdown button used to be .Selected = True tried changed to a button press.
            cbSelect.SetFocus();
            cbSelect.Press();
            
            //insert short desc in field
            GuiTextField shortDesc = (GuiTextField)session.FindById("wnd[0]/usr/subSCREEN_1:SAPLIQS0:1050/txtVIQMEL-QMTXT");
            shortDesc.Text = jobDetails.ShortDesc;

            //Reported by
            GuiTextField reportedBy = (GuiTextField)session.FindById("wnd[0]/usr/tabsTAB_GROUP_10/tabp10\\TAB01/ssubSUB_GROUP_10:SAPLIQS0:7235/subCUSTOM_SCREEN:SAPLIQS0:7212/subSUBSCREEN_2:SAPLIQS0:7326/ctxtRIWO00-GEWRK");       
            reportedBy.Text = jobDetails.Title;

            //Name?
            GuiTextField name = (GuiTextField)session.FindById("wnd[0]/usr/tabsTAB_GROUP_10/tabp10\\TAB01/ssubSUB_GROUP_10:SAPLIQS0:7235/subCUSTOM_SCREEN:SAPLIQS0:7212/subSUBSCREEN_2:SAPLIQS0:7326/ctxtVIQMEL-QMNAM");       
            name.Text = jobDetails.Title;

            temp = (GuiTextField)session.FindById("wnd[0]/usr/subSCREEN_1:SAPLIQS0:1050/txtVIQMEL-QMTXT");
            temp.CaretPosition = 4;
            
            frame.SendVKey(0);
            
            frame.SendVKey(0);

            GuiTextedit guiTextEdit = (GuiTextedit)session.FindById("wnd[0]/usr/tabsTAB_GROUP_10/tabp10\\TAB01/ssubSUB_GROUP_10:SAPLIQS0:7235/subCUSTOM_SCREEN:SAPLIQS0:7212/subSUBSCREEN_5:SAPLIQS0:7715/cntlTEXT/shellcont/shell");
            guiTextEdit.SetSelectionIndexes(0, 0);

            GuiTextField longDesc = (GuiTextField)session.FindById("wnd[0]/usr/tabsTAB_GROUP_10/tabp10\\TAB01/ssubSUB_GROUP_10:SAPLIQS0:7235/subCUSTOM_SCREEN:SAPLIQS0:7212/subSUBSCREEN_5:SAPLIQS0:7715/cntlTEXT/shellcont/shell");
            longDesc.Text = jobDetails.LongDesc.ToString();

            //JSEA ID number Written to Text Field;
            GuiTextField JseaID = (GuiTextField)session.FindById("wnd[0]/usr/tabsTAB_GROUP_10/tabp10\\TAB01/ssubSUB_GROUP_10:SAPLIQS0:7235/subCUSTOM_SCREEN:SAPLIQS0:7212/subSUBSCREEN_4:SAPLIQS0:7324/txtVIQMFE-FETXT");       
            JseaID.Text = jobDetails.JseaID.ToString();

            guiTextEdit = (GuiTextedit)session.FindById("wnd[0]/usr/tabsTAB_GROUP_10/tabp10\\TAB01/ssubSUB_GROUP_10:SAPLIQS0:7235/subCUSTOM_SCREEN:SAPLIQS0:7212/subSUBSCREEN_5:SAPLIQS0:7715/cntlTEXT/shellcont/shell");
            guiTextEdit.SetSelectionIndexes(29, 29);
            
            frame.SendVKey(4);

            btn = (GuiButton)session.FindById("wnd[0]/usr/subSCREEN_1:SAPLIQS0:1050/btnXICON_ORDER");
            btn.SetFocus();
            btn.Press();

            btn = (GuiButton)session.FindById("wnd[1]/tbar[0]/btn[0]");
            btn.SetFocus();
            btn.Press();

            btn = (GuiButton)session.FindById("wnd[0]/tbar[1]/btn[25]");
            btn.SetFocus();
            btn.Press();

            btn = (GuiButton)session.FindById("wnd[0]/tbar[0]/btn[11]");
            btn.SetFocus();
            btn.Press();

            //open time confirmation page
            temp = (GuiTextField)session.FindById("wnd[0]/tbar[0]/okcd");
            temp.Text = "/niw42";

            frame.SendVKey(0);
            frame.SendVKey(0);

            // Time input
            GuiTextField time = (GuiTextField)session.FindById("wnd[0]/usr/subSUB1:SAPLCMFU:0011/subSUB11_1:SAPLCMFU:0101/subOBJECTSCREEN_01:SAPLCORU:3360/tblSAPLCORUTABCNTR_3360/txtAFRUD-ISMNW_2[4,0]");
            time.Text = jobDetails.Time;

            temp = (GuiTextField)session.FindById("wnd[0]/usr/subSUB1:SAPLCMFU:0011/subSUB11_1:SAPLCMFU:0101/subOBJECTSCREEN_01:SAPLCORU:3360/tblSAPLCORUTABCNTR_3360/txtAFRUD-ISMNW_2[4,0]");
            temp.SetFocus();
            temp.CaretPosition = 1;
    
            // Additional worker one
            if (!jobDetails.AdditionalWorkerOne.Equals(""))
            {
                temp = (GuiTextField)session.FindById("wnd[0]/usr/subSUB1:SAPLCMFU:0011/subSUB11_1:SAPLCMFU:0101/subOBJECTSCREEN_01:SAPLCORU:3360/tblSAPLCORUTABCNTR_3360/txtAFRUD-VORNR[1,1]");
                temp.Text = "0010";

                GuiTextField additionalWorkerOne = (GuiTextField)session.FindById("wnd[0]/usr/subSUB1:SAPLCMFU:0011/subSUB11_1:SAPLCMFU:0101/subOBJECTSCREEN_01:SAPLCORU:3360/tblSAPLCORUTABCNTR_3360/ctxtAFRUD-ARBPL[9,1]");
                additionalWorkerOne.Text = jobDetails.AdditionalWorkerOne.ToString();

                time = (GuiTextField)session.FindById("wnd[0]/usr/subSUB1:SAPLCMFU:0011/subSUB11_1:SAPLCMFU:0101/subOBJECTSCREEN_01:SAPLCORU:3360/tblSAPLCORUTABCNTR_3360/txtAFRUD-ISMNW_2[4,1]");
                time.Text = jobDetails.Time;
            }

            // Additional worker two
            if (!jobDetails.AdditionalWorkerTwo.Equals(""))
            {
                temp = (GuiTextField)session.FindById("wnd[0]/usr/subSUB1:SAPLCMFU:0011/subSUB11_1:SAPLCMFU:0101/subOBJECTSCREEN_01:SAPLCORU:3360/tblSAPLCORUTABCNTR_3360/txtAFRUD-VORNR[1,2]");
                temp.Text = "0010";

                GuiTextField additionalWorkerOne = (GuiTextField)session.FindById("wnd[0]/usr/subSUB1:SAPLCMFU:0011/subSUB11_1:SAPLCMFU:0101/subOBJECTSCREEN_01:SAPLCORU:3360/tblSAPLCORUTABCNTR_3360/ctxtAFRUD-ARBPL[9,2]");
                additionalWorkerOne.Text = jobDetails.AdditionalWorkerTwo.ToString();

                time = (GuiTextField)session.FindById("wnd[0]/usr/subSUB1:SAPLCMFU:0011/subSUB11_1:SAPLCMFU:0101/subOBJECTSCREEN_01:SAPLCORU:3360/tblSAPLCORUTABCNTR_3360/txtAFRUD-ISMNW_2[4,2]");
                time.Text = jobDetails.Time;
            }

            // Booking parts out - 1st part
            if (!jobDetails.PartNumOne.Equals("") && !jobDetails.PartQtyOne.Equals(""))
            {
                GuiTextField partNumOne = (GuiTextField)session.FindById("wnd[0]/usr/subSUB2:SAPLCMFU:0021/subSUB21_1:SAPLCMFU:0103/subOBJECTSCREEN_03:SAPLCOWB:0515/tblSAPLCOWBTCTRL_0515/ctxtCOWB_COMP-MATNR[0,0]");
                partNumOne.Text = jobDetails.PartNumOne.ToString();

                GuiTextField partQtyOne = (GuiTextField)session.FindById("wnd[0]/usr/subSUB2:SAPLCMFU:0021/subSUB21_1:SAPLCMFU:0103/subOBJECTSCREEN_03:SAPLCOWB:0515/tblSAPLCOWBTCTRL_0515/txtCOWB_COMP-ERFMG[2,0]");
                partQtyOne.Text = jobDetails.PartQtyOne.ToString();

                temp = (GuiTextField)session.FindById("wnd[0]/usr/subSUB2:SAPLCMFU:0021/subSUB21_1:SAPLCMFU:0103/subOBJECTSCREEN_03:SAPLCOWB:0515/tblSAPLCOWBTCTRL_0515/ctxtCOWB_COMP-WERKS[4,0]");
                temp.Text = "7003";
                temp = (GuiTextField)session.FindById("wnd[0]/usr/subSUB2:SAPLCMFU:0021/subSUB21_1:SAPLCMFU:0103/subOBJECTSCREEN_03:SAPLCOWB:0515/tblSAPLCOWBTCTRL_0515/ctxtCOWB_COMP-LGORT[5,0]");
                temp.Text = "5000";

            }

            // Booking parts out - 2nd part
            if (!jobDetails.PartNumTwo.Equals("") && !jobDetails.PartQtyTwo.Equals(""))
            {
                GuiTextField partNumTwo = (GuiTextField)session.FindById("wnd[0]/usr/subSUB2:SAPLCMFU:0021/subSUB21_1:SAPLCMFU:0103/subOBJECTSCREEN_03:SAPLCOWB:0515/tblSAPLCOWBTCTRL_0515/ctxtCOWB_COMP-MATNR[0,1]");
                partNumTwo.Text = jobDetails.PartNumTwo.ToString();

                GuiTextField partQtyTwo = (GuiTextField)session.FindById("wnd[0]/usr/subSUB2:SAPLCMFU:0021/subSUB21_1:SAPLCMFU:0103/subOBJECTSCREEN_03:SAPLCOWB:0515/tblSAPLCOWBTCTRL_0515/txtCOWB_COMP-ERFMG[2,1]");
                partQtyTwo.Text = jobDetails.PartQtyTwo.ToString();

                temp = (GuiTextField)session.FindById("wnd[0]/usr/subSUB2:SAPLCMFU:0021/subSUB21_1:SAPLCMFU:0103/subOBJECTSCREEN_03:SAPLCOWB:0515/tblSAPLCOWBTCTRL_0515/ctxtCOWB_COMP-WERKS[4,1]");
                temp.Text = "7003";
                temp = (GuiTextField)session.FindById("wnd[0]/usr/subSUB2:SAPLCMFU:0021/subSUB21_1:SAPLCMFU:0103/subOBJECTSCREEN_03:SAPLCOWB:0515/tblSAPLCOWBTCTRL_0515/ctxtCOWB_COMP-LGORT[5,1]");
                temp.Text = "5000";
            }

            temp = (GuiTextField)session.FindById("wnd[0]/usr/subSUB1:SAPLCMFU:0011/subSUB11_1:SAPLCMFU:0101/subOBJECTSCREEN_01:SAPLCORU:3360/tblSAPLCORUTABCNTR_3360/ctxtAFRUD-ARBPL[9,2]");
            temp.SetFocus();
            temp.CaretPosition = 7;            
            frame.SendVKey(0);

            btn = (GuiButton)session.FindById("wnd[0]/usr/subHEADER:SAPLCMFU:0201/btnHEADER_TECO");
            btn.Press();

            btn = (GuiButton)session.FindById("wnd[0]/tbar[0]/btn[11]");
            btn.SetFocus();
            btn.Press();

            // Return to main page.
            temp = (GuiTextField)session.FindById("wnd[0]/tbar[0]/okcd");
            temp.Text = "/n";

            frame.SendVKey(0);
        }
    }
}
