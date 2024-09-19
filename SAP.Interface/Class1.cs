using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPConnect;

namespace SAP.Interface
{
    public class Material 
    {
        public string MATART  { get; set; }
        public string MATNR   { get; set; }
        public string ZZERZNR { get; set; }
        public string MEINS   { get; set; }
        public string GEWEI   { get; set; }
        public string MSTAE { get; set; }
        public string BENID { get; set; }
        public string WRKST { get; set; }
        public string WRKSTV { get; set; }
        public string ANLUSER { get; set; }
        public string BEREICH { get; set; }
        public string EMBARGO { get; set; }
        public string LOCATION { get; set; }
        public string INT_COMM { get; set; }
        public string SETWEIGHT { get; set; }  

        public Material(string _MATART, string _MATNR, string _ZZERZNR, string _MEINS, string _GEWEI, string _MSTAE, string _BENID, string _WRKST, string _WRKSTV, string _ANLUSER, string _BEREICH, string _EMBARGO, string _LOCATION, string _INT_COMM, string _SETWEIGHT)
        {
            this.MATART = _MATART;
            this.MATNR = _MATNR;
            this.ZZERZNR = _ZZERZNR;
            this.MEINS = _MEINS;
            this.GEWEI = _GEWEI;
            this.MSTAE = _MSTAE;
            this.BENID = _BENID;
            this.WRKST = _WRKST;
            this.WRKSTV = _WRKSTV;
            this.ANLUSER = _ANLUSER;
            this.BEREICH = _BEREICH;
            this.EMBARGO = _EMBARGO;
            this.LOCATION = _LOCATION;
            this.INT_COMM = _INT_COMM;
            this.SETWEIGHT = _SETWEIGHT;
            
        }

        public static List<ReturnMessage> TransferBOM(R3Connection con, string matnr, string valid_from, List<BOM_Items> bom, List<BOM_Items_Text> bom_text)
        {
            ERPConnect.LIC.SetLic("WPD6FP6607");

            int flag_error = 0;

            List<ReturnMessage> result = new List<ReturnMessage>();


            con.Open(false);

            // Create a function object  
            RFCFunction func = con.CreateFunction("Z_MM_BOM_CREATE");

            //Import-Struktur IS_MATNR
            func.Exports["I_MATNR"].ParamValue = matnr;
            func.Exports["I_VALID_FROM"].ParamValue = valid_from;

            int count = 0;

             foreach (BOM_Items bomit in bom)
                {
                    if (count == 0) ;
                    RFCTable tbText = func.Tables["IT_POS"];

                    RFCStructure recAttr = tbText.AddRow();

                    recAttr["ITEM_NO"] = bomit.line_no;
                    recAttr["COMPONENT"] = bomit.item_number.Trim().PadLeft(18, '0');
                    recAttr["COMP_QTY"] = bomit.item_qty;
                    recAttr["COMP_UNIT"] = bomit.item_unit;

                    count++;
                }

             count = 0;

                foreach (BOM_Items_Text bomit_t in bom_text)
                {
                    if (count == 0) ;
                    RFCTable tbText = func.Tables["IT_TEXT"];

                    RFCStructure recAttr = tbText.AddRow();

                    recAttr["IDENTIFIER"] = bomit_t.identifier;
                    recAttr["TDLINE"] = bomit_t.text;


                    count++;
                }
             ReturnMessage row_result = new ReturnMessage();

             func.Execute();
             RFCStructure es_result = func.Imports["e_bapiret2"].ToStructure();
             row_result.TYPE = es_result["TYPE"].ToString();
             row_result.ID = es_result["ID"].ToString();
             row_result.NUMBER = es_result["NUMBER"].ToString();
             row_result.MESSAGE = es_result["MESSAGE"].ToString();
             row_result.LOG_NO = es_result["LOG_NO"].ToString();
             row_result.LOG_MSG_NO = es_result["LOG_MSG_NO"].ToString();
             row_result.MESSAGE_V1 = es_result["MESSAGE_V1"].ToString();
             row_result.MESSAGE_V2 = es_result["MESSAGE_V2"].ToString();
             row_result.MESSAGE_V3 = es_result["MESSAGE_V3"].ToString();
             row_result.MESSAGE_V4 = es_result["MESSAGE_V4"].ToString();
             row_result.PARAMETER = es_result["PARAMETER"].ToString();
             row_result.ROW = es_result["ROW"].ToString();
             row_result.FIELD = es_result["FIELD"].ToString();
             row_result.SYSTEM = es_result["SYSTEM"].ToString();

             result.Add(row_result);

            return result;
        }

        public static List<ReturnMessage> SendWerkstoffProd(R3Connection con, List<WerkstoffProd> WerkstoffProd)
        {
            ERPConnect.LIC.SetLic("WPD6FP6607");
            con.Open(false);

            List<ReturnMessage> result = new List<ReturnMessage>();

            RFCFunction func = con.CreateFunction("Z_CREATE_WERKSTOFFBEZ_FERT");

            RFCTable tbOrgText = func.Tables["IT_WRKT"];
            foreach (WerkstoffProd prod in WerkstoffProd)
            {
                RFCStructure recSigOrg = tbOrgText.AddRow();

                recSigOrg["ZZWRKST"] = prod.ID;
                recSigOrg["SPRAS"] = prod.LANGU;
                recSigOrg["ZWRKSTTX"] = prod.WERKSTOFF_DESC;
            }

            try
            {
                ReturnMessage row_result = new ReturnMessage();

                func.Execute();
                RFCStructure es_result = func.Imports["e_bapiret2"].ToStructure();
                row_result.TYPE = es_result["TYPE"].ToString();
                row_result.ID = es_result["ID"].ToString();
                row_result.NUMBER = es_result["NUMBER"].ToString();
                row_result.MESSAGE = es_result["MESSAGE"].ToString();
                row_result.LOG_NO = es_result["LOG_NO"].ToString();
                row_result.LOG_MSG_NO = es_result["LOG_MSG_NO"].ToString();
                row_result.MESSAGE_V1 = es_result["MESSAGE_V1"].ToString();
                row_result.MESSAGE_V2 = es_result["MESSAGE_V2"].ToString();
                row_result.MESSAGE_V3 = es_result["MESSAGE_V3"].ToString();
                row_result.MESSAGE_V4 = es_result["MESSAGE_V4"].ToString();
                row_result.PARAMETER = es_result["PARAMETER"].ToString();
                row_result.ROW = es_result["ROW"].ToString();
                row_result.FIELD = es_result["FIELD"].ToString();
                row_result.SYSTEM = es_result["SYSTEM"].ToString();

                result.Add(row_result);

            }
            catch (ERPException e)
            {
                ReturnMessage row_result = new ReturnMessage("E", "", "", "", "", "", e.Message, e.ABAPException, "", "", "", "", "", "");

                result.Add(row_result);
            }
            return result;

        }

            public static List<ReturnMessage> SendWerkstoffVertrieb(R3Connection con, List<WerkstoffVertrieb> WerkstoffVertrieb)
            {
                ERPConnect.LIC.SetLic("WPD6FP6607");
                con.Open(false);

                List<ReturnMessage> result = new List<ReturnMessage>();

                RFCFunction func = con.CreateFunction("Z_CREATE_WERKSTOFFBEZ_VERTRIEB");

                RFCTable tbOrgText = func.Tables["IT_WRKT"];
                foreach (WerkstoffVertrieb prod in WerkstoffVertrieb)
                {
                    RFCStructure recSigOrg = tbOrgText.AddRow();

                    recSigOrg["ZZWRKSTV"] = prod.ID;
                    recSigOrg["SPRAS"] = prod.LANGU;
                    recSigOrg["ZWRKSTTX"] = prod.WERKSTOFF_DESC;
                }

                try
                {
                    ReturnMessage row_result = new ReturnMessage();

                    func.Execute();
                    RFCStructure es_result = func.Imports["e_bapiret2"].ToStructure();
                    row_result.TYPE = es_result["TYPE"].ToString();
                    row_result.ID = es_result["ID"].ToString();
                    row_result.NUMBER = es_result["NUMBER"].ToString();
                    row_result.MESSAGE = es_result["MESSAGE"].ToString();
                    row_result.LOG_NO = es_result["LOG_NO"].ToString();
                    row_result.LOG_MSG_NO = es_result["LOG_MSG_NO"].ToString();
                    row_result.MESSAGE_V1 = es_result["MESSAGE_V1"].ToString();
                    row_result.MESSAGE_V2 = es_result["MESSAGE_V2"].ToString();
                    row_result.MESSAGE_V3 = es_result["MESSAGE_V3"].ToString();
                    row_result.MESSAGE_V4 = es_result["MESSAGE_V4"].ToString();
                    row_result.PARAMETER = es_result["PARAMETER"].ToString();
                    row_result.ROW = es_result["ROW"].ToString();
                    row_result.FIELD = es_result["FIELD"].ToString();
                    row_result.SYSTEM = es_result["SYSTEM"].ToString();

                    result.Add(row_result);

                }
                catch (ERPException e)
                {
                    ReturnMessage row_result = new ReturnMessage("E", "", "", "", "", "", e.Message, e.ABAPException, "", "", "", "", "", "");

                    result.Add(row_result);
                }
                return result;
            }

            public static List<ReturnMessage> ChangeMaterial(R3Connection con, Material matnr, List<Classifications> Class, List<GeneralAttributes> GenAttr, List<Longtext> Longt)
        {
            ERPConnect.LIC.SetLic("WPD6FP6607");

            int flag_error = 0;

            List<ReturnMessage> result = new List<ReturnMessage>();
            
                con.Open(false);
                
                // Create a function object  
                RFCFunction func = con.CreateFunction("Z_MM_MAT_CREATE");

                //Import-Struktur IS_MATNR
                RFCStructure data_specific = func.Exports["IS_MATNR"].ToStructure();

                data_specific["MTART"] = matnr.MATART;

                // Wenn X dann wird Nettogewicht gesetzt
                data_specific["SET_WEIGHT"] = matnr.SETWEIGHT;

                //Falls eine Materialnummer übergeben wird, wird es auf 18 Stellen mit führender Null normiert
                if (matnr.MATNR != "")
                    data_specific["MATNR"] = matnr.MATNR.ToString().PadLeft(18, '0');

                //Erzeugnisnummer
                if (matnr.ZZERZNR != "")
                    data_specific["ZZERZNR"] = matnr.ZZERZNR.ToString();

                //Mengeneinheit
                //if (SAP.Interface.Util.checkMein(con, "DE", matnr.MEINS) == 1)
                    data_specific["MEINS"] = matnr.MEINS;
                //else
                //{
                    //Fehler
                //    ReturnMessage row_result = new ReturnMessage("E", "", "", "", "", "", "Mengeneinheit "+matnr.MEINS +" nicht bekannt", "", "", "", "", "", "", "");
                //    result.Add(row_result);
                //    flag_error = 1;
                //}

                //Gewichtseinheit
                //if (SAP.Interface.Util.checkMein(con, "DE", matnr.GEWEI) == 1)
                    data_specific["GEWEI"] = matnr.GEWEI;
                //else
                //{
                    //Fehler
                //    ReturnMessage row_result = new ReturnMessage("E", "", "", "", "", "", "Gewichtseinheit " + matnr.GEWEI + " nicht bekannt", "", "", "", "", "", "", "");
                //    result.Add(row_result);
                //    flag_error = 1;
                //}

                //Materialstatus
                data_specific["MSTAE"] = matnr.MSTAE.ToString();

                //Benenungkatalog
                data_specific["ZZBENID"] = matnr.BENID.ToString();

                //Werkstoff
                data_specific["ZZWRKST"] = matnr.WRKST.ToString();

                //Werkstoff-Vertrieb
                data_specific["ZZWRKSTV"] = matnr.WRKSTV.ToString();

                //Anlegender Benutzer
                data_specific["ZZANLBP"] = matnr.ANLUSER.ToString();

                //Bereich
                data_specific["ZZBEREP"] = matnr.BEREICH.ToString();

                //Embargo
                data_specific["ZZEMBGO"] = matnr.EMBARGO.ToString();

                //Standort
                data_specific["ZZLOCTN"] = matnr.LOCATION.ToString();

                data_specific["ZZINKOP"] = matnr.INT_COMM.ToString();

                int count = 0;
                /*foreach (Material_Text text in MatText)
                {
                    if (count == 0) ;
                    RFCTable tbText = func.Tables["IT_MAKTX"];

                    RFCStructure recSig = tbText.AddRow();

                    recSig["LANGU"] = text.LANGU;
                    recSig["LANGU_ISO"] = text.LANGU_ISO;
                    recSig["MATL_DESC"] = text.MATL_DESC;

                    count++;
                }*/

                count = 0;
            foreach (Classifications classi in Class)
                {
                    if (count == 0) ;
                    RFCTable tbText = func.Tables["IT_CLASS"];

                    RFCStructure recSig = tbText.AddRow();

                    recSig["CLASSID"] = classi.CLASSID;
                    recSig["CLASSNAME"] = classi.CLASSNAME;
                    recSig["ATTRIBUTEID"] = classi.ATTRIBUTEID;
                    recSig["ATTRIBUTENAME"] = classi.ATTRIBUTENAME;
                    recSig["ATRRIBUTEVALUE"] = classi.ATTRIBUTEVALUE;
                    recSig["ATTRIBUTETYPE"] = classi.ATTRIBUTETYPE;


                    count++;
                }

            count = 0;
            foreach (Longtext longt in Longt)
                {
                    if (count == 0) ;
                    RFCTable tbText = func.Tables["IT_TEXT"];

                    RFCStructure recTxt = tbText.AddRow();

                    recTxt["TDFORMAT"] = "*";
                    recTxt["TDLINE"] = longt.text;
                    count++;
                }


            foreach (GeneralAttributes genattr in GenAttr)
                {
                    if (count == 0) ;
                    RFCTable tbText = func.Tables["IT_ATTRIBUTES"];

                    RFCStructure recAttr = tbText.AddRow();

                    recAttr["ATTRIBUTENAME"] = genattr.ATTRIBUTENAME;
                    recAttr["ATTRIBUTEVALUE"] = genattr.ATTRIBUTEVALUE;

                    count++;
                }

                //Kein Fehler bei der Prüfung
                if (flag_error == 0)
                {
                    try
                    {
                        ReturnMessage row_result = new ReturnMessage();

                        func.Execute();
                        RFCStructure es_result = func.Imports["e_bapiret2"].ToStructure();
                        row_result.TYPE = es_result["TYPE"].ToString();
                        row_result.ID = es_result["ID"].ToString();
                        row_result.NUMBER = es_result["NUMBER"].ToString();
                        row_result.MESSAGE = es_result["MESSAGE"].ToString();
                        row_result.LOG_NO = es_result["LOG_NO"].ToString();
                        row_result.LOG_MSG_NO = es_result["LOG_MSG_NO"].ToString();
                        row_result.MESSAGE_V1 = es_result["MESSAGE_V1"].ToString();
                        row_result.MESSAGE_V2 = es_result["MESSAGE_V2"].ToString();
                        row_result.MESSAGE_V3 = es_result["MESSAGE_V3"].ToString();
                        row_result.MESSAGE_V4 = es_result["MESSAGE_V4"].ToString();
                        row_result.PARAMETER = es_result["PARAMETER"].ToString();
                        row_result.ROW = es_result["ROW"].ToString();
                        row_result.FIELD = es_result["FIELD"].ToString();
                        row_result.SYSTEM = es_result["SYSTEM"].ToString();

                        result.Add(row_result);

                    }
                    catch (ERPException e)
                    {
                        ReturnMessage row_result = new ReturnMessage("E", "", "", "", "", "", e.Message, e.ABAPException, "", "", "", "", "", "");

                        result.Add(row_result);
                    }
                    return result;
                }//Flag_error == 0
                else
                {
                    return result;
                }            
        }           

        public static Boolean CheckMatnrProdnr(R3Connection con, Material matnr)
        {
            ERPConnect.LIC.SetLic("WPD6FP6607");
            con.Open(false);
            RFCFunction func = con.CreateFunction("Z_MM_MAT_UNIQUE");

            try
            {
                func.Exports["I_MATNR"].ParamValue = matnr.MATNR.PadLeft(18, '0');
                func.Exports["I_PRODNR"].ParamValue = matnr.ZZERZNR;
                func.Execute();
            }
            catch (ERPException e)
            {
                ;
            }

            if (func.Imports["E_VALID"].ToString() == "X")
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }            
        }
    }

    public class Document
    {
        public string DOCUMENTTYPE  { get; set; }
        public Int32 DOCUMENTNUMBER { get; set; }
        public string DOCUMENTVERSION { get; set; }
        public string STATUSINTERN { get; set; }
        public string LOCATION { get; set; }
        public string LWN { get; set; }

        public Document(string _DOCUMENTTYPE, Int32 _DOCUMENTNUMBER, string _DOCUMENTVERSION, string _STATUSINTERN, string _LOCATION, string _LWN)
        {
            this.DOCUMENTTYPE = _DOCUMENTTYPE;
            this.DOCUMENTNUMBER = _DOCUMENTNUMBER;
            this.DOCUMENTVERSION = _DOCUMENTVERSION;
            this.STATUSINTERN = _STATUSINTERN;
            this.LOCATION = _LOCATION;
            this.LWN = _LWN;
        }
             

        public static List<ReturnMessage> ChangeDocument(R3Connection con, Material matnr, Document doc, List<Document_Text> DocText,  List<Document_Originals> DocOrigs, List<Mara> mara)
        {
            ERPConnect.LIC.SetLic("WPD6FP6607");

            int flag_error = 0;

            List<ReturnMessage> result = new List<ReturnMessage>();
            //con.LogDir = @"C:\Temp\Trace";    //  (directory must exist)
            //con.Logging = true;
                 con.Open(false);

                // Create a function object  
                RFCFunction func = con.CreateFunction("Z_MM_DOC_CREATE");

                //Import-Struktur IS_BAPI_DOC_DRAW2 - Kopfdaten des Dokuments
                RFCStructure data_is_bapi = func.Exports["IS_BAPI_DOC_DRAW2"].ToStructure();

                func.Exports["I_LOCTN"].ParamValue = doc.LOCATION;
                func.Exports["I_WERKN"].ParamValue = doc.LWN;

                data_is_bapi["DOCUMENTTYPE"] = doc.DOCUMENTTYPE;
                data_is_bapi["DOCUMENTNUMBER"] = doc.DOCUMENTNUMBER;
                data_is_bapi["DOCUMENTVERSION"] = doc.DOCUMENTVERSION;
                data_is_bapi["DESCRIPTION"] = "Fertigungszeichnung";
                data_is_bapi["STATUSINTERN"] = doc.STATUSINTERN;
                            
                //Falls eine Materialnummer übergeben wird, wird es auf 18 Stellen mit führender Null normiert
                //if (matnr.MATNR != "")
                //    func.Exports["I_MATNR"].ParamValue = matnr.MATNR.ToString().PadLeft(18, '0');

                int count = 0;
                RFCTable tbOrgText = func.Tables["IT_ORGTX"];
                foreach (Document_Originals orig in DocOrigs)
                {
                    RFCStructure recSigOrg = tbOrgText.AddRow();

                    recSigOrg["STORAGECATEGORY"] = orig.STORAGECATEGORY;
                    recSigOrg["WSAPPLICATION"] = orig.WSAPPLICATION;
                    recSigOrg["DOCPATH"] = orig.DOCPATH;
                    recSigOrg["DOCFILE"]= orig.DOCFILE;
                    recSigOrg["DESCRIPTION"] = orig.DESCRIPTION;

                    count++;
                }
   
                 count = 0;
                RFCTable tbDokText = func.Tables["IT_DOKKTX"];
                foreach (Document_Text text in DocText)
                {
                    RFCStructure recSigDok = tbDokText.AddRow();

                    recSigDok["LANGU"] = text.LANGU;
                    recSigDok["LANGU_ISO"] = text.LANGU_ISO;
                    recSigDok["MATL_DESC"] = text.DOC_DESC;

                    count++;
                }

                count = 0;
                RFCTable tbMat = func.Tables["IT_MARA"];
                foreach (Mara mat in mara)
                {
                    RFCStructure recSigMat = tbMat.AddRow();

                    recSigMat["MATNR"] = mat.MATNR.ToString().PadLeft(18, '0');
                    
                    count++;
                }

                    try
                    {
                        ReturnMessage row_result = new ReturnMessage();

                        func.Execute();
                        RFCStructure es_result = func.Imports["e_bapiret2"].ToStructure();
                        
                        row_result.TYPE = es_result["TYPE"].ToString();
                        row_result.ID = es_result["ID"].ToString();
                        row_result.NUMBER = es_result["NUMBER"].ToString();
                        row_result.MESSAGE = es_result["MESSAGE"].ToString();
                        row_result.LOG_NO = es_result["LOG_NO"].ToString();
                        row_result.LOG_MSG_NO = es_result["LOG_MSG_NO"].ToString();
                        row_result.MESSAGE_V1 = es_result["MESSAGE_V1"].ToString();
                        row_result.MESSAGE_V2 = es_result["MESSAGE_V2"].ToString();
                        row_result.MESSAGE_V3 = es_result["MESSAGE_V3"].ToString();
                        row_result.MESSAGE_V4 = es_result["MESSAGE_V4"].ToString();
                        row_result.PARAMETER = es_result["PARAMETER"].ToString();
                        row_result.ROW = es_result["ROW"].ToString();
                        row_result.FIELD = es_result["FIELD"].ToString();
                        row_result.SYSTEM = es_result["SYSTEM"].ToString();

                        result.Add(row_result);

                    }
                    catch (ERPException e)
                    {
                        ReturnMessage row_result = new ReturnMessage("E", "", "", "", "", "", e.Message, e.ABAPException, "", "", "", "", "", "");

                        result.Add(row_result);
                    }
                    return result;          
        }         
    }

     public class Document_Text
    {
        public string LANGU { get; set; }
        public string LANGU_ISO { get; set; }
        public string DOC_DESC { get; set; }
    }

     public class Mara
     {
         public string MATNR { get; set; }
     }

     public class Document_Originals
     {
         public string STORAGECATEGORY { get; set; }
         public string WSAPPLICATION { get; set; }
         public string DOCPATH { get; set; }
         public string DOCFILE { get; set; }
         public string DESCRIPTION { get; set; }
     }

    public class Material_Text
    {
        public string LANGU { get; set; }
        public string LANGU_ISO { get; set; }
        public string MATL_DESC { get; set; }
    }


    public class Longtext
    {
        public string text { get; set; }
    }

    public class BOM_Items
    {
        public string line_no { get; set; }
        public string item_number { get; set; }
        public float item_qty { get; set; }
        public string item_unit { get; set; }
    }

    public class BOM_Items_Text
    {
        public string identifier { get; set; }
        public string text { get; set; }
    }

    public class Classifications
    {
        public string CLASSID { get; set; }
        public string CLASSNAME { get; set; }
        public string ATTRIBUTEID { get; set; }
        public string ATTRIBUTENAME { get; set; }
        public string ATTRIBUTEVALUE { get; set; }
        public string ATTRIBUTETYPE { get; set; }
    }

    public class WerkstoffProd
    {
        public string ID { get; set; }
        public string LANGU { get; set; }
        public string WERKSTOFF_DESC { get; set; }
    }

    public class WerkstoffVertrieb
    {
        public string ID { get; set; }
        public string LANGU { get; set; }
        public string WERKSTOFF_DESC { get; set; }
    }

    public class GeneralAttributes
    {
        public string ATTRIBUTENAME { get; set; }
        public string ATTRIBUTEVALUE { get; set; }
    }

    public class ReturnMessage
    {
        public string TYPE { get; set; }
        public string ID { get; set; }
        public string NUMBER { get; set; }
        public string MESSAGE { get; set; }
        public string LOG_NO { get; set; }
        public string LOG_MSG_NO { get; set; }
        public string MESSAGE_V1 { get; set; }
        public string MESSAGE_V2 { get; set; }
        public string MESSAGE_V3 { get; set; }
        public string MESSAGE_V4 { get; set; }
        public string PARAMETER { get; set; }
        public string ROW { get; set; }
        public string FIELD { get; set; }
        public string SYSTEM { get; set; }

        public ReturnMessage()
        {

        }

        public ReturnMessage (string _TYPE, string _ID, string _NUMBER, string _MESSAGE, string _LOG_NO, string _LOG_MSG_NO, string _MESSAGE_V1, string _MESSAGE_V2, string _MESSAGE_V3, string _MESSAGE_V4, string _PARAMETER, string _ROW, string _FIELD, string _SYSTEM)
        {
            this.TYPE = _TYPE;
            this.ID = _ID;
            this.NUMBER = _NUMBER;
            this.MESSAGE = _MESSAGE;
            this.LOG_NO = _LOG_NO;
            this.LOG_MSG_NO = _LOG_MSG_NO;
            this.MESSAGE_V1 = _MESSAGE_V1;
            this.MESSAGE_V2 = _MESSAGE_V2;
            this.MESSAGE_V3 = _MESSAGE_V3;
            this.MESSAGE_V4 = _MESSAGE_V4;
            this.PARAMETER = _PARAMETER;
            this.ROW = _ROW;
            this.FIELD = _FIELD;
            this.SYSTEM = _SYSTEM;
        }
    }
}
