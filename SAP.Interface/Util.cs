using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPConnect;
using ERPConnect.Utils;
using System.Data;

namespace SAP.Interface
{
    class Util
    {
        public static int checkMein(R3Connection con, string spras, string msehi)
        {
            con.Open(false);
            ReadTable table = new ReadTable(con);
            table.AddField("MSEHI");              
           // table. AddCriteria("SPRAS = '"+spras+"'");
           // table.AddCriteria("AND MSEHI = '"+msehi+"'");
            table.WhereClause = "SPRAS = '" + spras + "'" + " AND MSEHI = '" + msehi + "'";
            table.TableName = "T006A";
            table.RowCount = 10;

            table.Run();

            DataTable resulttable = table.Result;

            return resulttable.Rows.Count;

        }
    }
}
