using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Touha.Utils.DBExportDoc
{
    public partial class Form1 : Form
    {
        #region Fields
        private string _connectionString;

        private DbConnection _connection;

        private DataSet _ds = new DataSet();

        private DbDataAdapter _adapter;

        private const string _sqlGetUserTables = "select * from user_tables";

        private const string _sqlGetTableColumns = @"SELECT A.COLUMN_NAME,
         NULLABLE,
         B.COMMENTS,
         DATA_DEFAULT,
         CASE
            WHEN DATA_PRECISION IS NULL
            THEN
               DATA_TYPE || '(' || DATA_LENGTH || ')'
            ELSE
               DATA_TYPE || '(' || DATA_LENGTH || ',' || DATA_PRECISION || ')'
         END
            DATA_TYPE,
         NVL((SELECT C.CONSTRAINT_TYPE
            FROM user_constraints C, user_cons_columns D
           WHERE C.constraint_name = D.constraint_name AND D.table_name = :tableName AND D.COLUMN_NAME = A.COLUMN_NAME AND C.CONSTRAINT_TYPE = 'P'), 'N')
            IS_PRIMARY
    FROM user_tab_columns A, user_col_comments B
   WHERE     A.Table_Name = :tableName
         AND A.TABLE_NAME = B.TABLE_NAME
         AND A.COLUMN_NAME = B.COLUMN_NAME";
//@"SELECT A.COLUMN_NAME,
//       NULLABLE,
//       B.COMMENTS,
//       CASE
//          WHEN DATA_PRECISION IS NULL
//          THEN
//             DATA_TYPE || '(' || DATA_LENGTH || ')'
//          ELSE
//             DATA_TYPE || '(' || DATA_LENGTH || ',' || DATA_PRECISION || ')'
//       END
//          DATA_TYPE
//  FROM user_tab_columns A, user_col_comments B
// WHERE     A.Table_Name = :tableName
//       AND A.TABLE_NAME = B.TABLE_NAME
//       AND A.COLUMN_NAME = B.COLUMN_NAME";

        private string _modelFilePath;

        private string _outputFilePath = System.Environment.CurrentDirectory + "/output.xml";

        private const string _startIdentifier = "${start}";

        private const string _endIdentifier = "${end}";

        private const string _startIdentifier2 = "${start2}";

        private const string _endIdentifier2 = "${end2}";

        private string _modelString;

        private string _modelString2;
        #endregion

        #region Constructors
        public Form1()
        {
            InitializeComponent();
            txtConnectionString.Text = ConfigurationManager.AppSettings["defaultConnectionString"];
        }
        #endregion

        #region Events
        /// <summary>
        /// 连接按钮点击，开启数据库连接，获取当前用户表信息，并填充到CheckedListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            _connectionString = txtConnectionString.Text.Trim();
            if (!string.IsNullOrEmpty(_connectionString))
            {
                _connection = new OracleConnection(_connectionString);
                _adapter = new OracleDataAdapter();
                GetUserTables();
                FillCkListBox();
            }
            else
            {
                MessageBox.Show("连接字符串为空");
            }
        }

        /// <summary>
        /// 导出按钮点击，检查前置条件，并根据选中的表，以及模板文件，进行最终文件的生成。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {

            var selectedItems = ckListBoxTables.CheckedItems;
            if (selectedItems == null || selectedItems.Count == 0)
            {
                MessageBox.Show("请先选择要导出的表");
            }
            else if (string.IsNullOrEmpty(_modelFilePath))
            {
                MessageBox.Show("请先选择模板文件");
            }
            else
            {

                GetTableColumns(selectedItems);

                Render();
            }

        }

        /// <summary>
        /// 选择模板文件按钮点击，打开文件对话框，选择模板文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFileSel_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _modelFilePath = dialog.FileName;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 对CheckedListBox进行数据绑定
        /// </summary>
        private void FillCkListBox()
        {
            ckListBoxTables.DataSource = _ds.Tables[0];
            ckListBoxTables.DisplayMember = "TABLE_NAME";
            ckListBoxTables.ValueMember = "TABLE_NAME";
        }

        /// <summary>
        /// 获取当前用户下的表信息
        /// </summary>
        private void GetUserTables()
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = _sqlGetUserTables;
            cmd.CommandType = CommandType.Text;
            _adapter.SelectCommand = cmd;
            _adapter.Fill(_ds, "USER_TABLES");
        }

        /// <summary>
        /// 获取选中表的列信息
        /// </summary>
        /// <param name="selectedItems"></param>
        private void GetTableColumns(System.Windows.Forms.CheckedListBox.CheckedItemCollection selectedItems)
        {
            ClearTables();
            for(int i = 0; i < selectedItems.Count; i++)
            {
                var item = selectedItems[i];
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                DbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = _sqlGetTableColumns;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter("tableName", ckListBoxTables.GetItemText(item)));
                _adapter.SelectCommand = cmd;
                _adapter.Fill(_ds, ckListBoxTables.GetItemText(item));
            }
        }

        /// <summary>
        /// 清理除dataset中出第一个datatable（CheckedListBox对应的数据源）之外的，datatable
        /// </summary>
        private void ClearTables()
        {
            for (int i = 1; i < _ds.Tables.Count; i++)
            {
                _ds.Tables.RemoveAt(i);
            }
        }

        /// <summary>
        /// 根据模板文件，渲染出最终的文档
        /// </summary>
        private void Render()
        {
            #region 表模板
            string content = File.ReadAllText(_modelFilePath);
            int startIndex = content.IndexOf(_startIdentifier) + _startIdentifier.Length;
            int endIndex = content.IndexOf(_endIdentifier);
            _modelString = content.Substring(startIndex, endIndex - startIndex);
            #endregion

            #region 行模板
            int startIndex2 = content.IndexOf(_startIdentifier2) + _startIdentifier2.Length;
            int endIndex2 = content.IndexOf(_endIdentifier2);
            _modelString2 = content.Substring(startIndex2, endIndex2 - startIndex2);
            #endregion 
            StringBuilder sb = new StringBuilder();
            //遍历所有表
            for (int i = 1; i < _ds.Tables.Count; i++)
            {
                string temp = _modelString;
                var table = _ds.Tables[i];
                StringBuilder sb2 = new StringBuilder();
                //遍历所有行
                foreach(DataRow row in table.Rows)
                {
                    string temp2 = _modelString2;
                    foreach (DataColumn col in table.Columns)
                    {
                        temp2 = temp2.Replace("${" + col.ColumnName + "}", row[col.ColumnName].ToString());
                    }
                    sb2.Append(temp2);
                }
                temp = temp.Replace(_modelString2, sb2.ToString());
                sb.Append(temp);
            }
            content = content.Replace(_modelString, sb.ToString()).Replace(_startIdentifier, "").Replace(_endIdentifier, "")
                .Replace(_startIdentifier2, "").Replace(_endIdentifier2, "");
            File.WriteAllText(_outputFilePath, content);
        }

        #endregion

    }
}
