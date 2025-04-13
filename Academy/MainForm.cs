using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;

namespace Academy
{
	public partial class MainForm : Form
	{
		Connector connector;

		Query[] queries = new Query[]
			{
				new Query("*", "Students"),
				new Query(
					"group_id,group_name,COUNT(stud_id),direction_name",
					"Students,Groups,Directions",
					"direction = direction_id AND [group] = group_id",
					"group_id, group_name, direction_name"
					),
				new Query("*", "Directions"),
				new Query("*", "Disciplines"),
				new Query("*", "Teachers"),

				//сюда 1 пункт
				new Query(
                        	"direction_name, COUNT(DISTINCT group_id) AS group_count, COUNT(stud_id) AS student_count",
	                        "Directions LEFT JOIN Groups ON Directions.direction_id = Groups.direction LEFT JOIN Students ON Groups.group_id = Students.[group]",
	                        "",
	                        "direction_name"
					//
                          ),
			};




		DataGridView[] tables;
		//	string[] status_messages = new string[]
		//	{
		//		"Количество студентов: ",
		//	"Количество групп: ",
		//	"Количество направлений: ",
		//	"Количество дисциплин: ",
		//		"Количество преподавателей: ",
		//	};
		//5
		string[] status_messages = new string[]
	{
	"Количество студентов: ",
	"Количество групп: ",
	"Количество направлений: ",
	"Количество дисциплин: ",
	"Количество преподавателей: ",
	"Статистика направлений: " // Новое сообщение
	};
//







		public MainForm()
		{
			InitializeComponent();

			//	tables = new DataGridView[]
			//	{
			//		dgvStudents,
			//		dgvGroups,
			//		dgvDirections,
			//		dgvDisciplines,
			//		dgvTeachers
			//	};
			//4
			tables = new DataGridView[]
	               {
					dgvStudents,
					dgvGroups,
					dgvDirections,
					dgvDisciplines,
					dgvTeachers,
					dgvDirectionStats // Новая таблица
				   };





			connector = new Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
			dgvStudents.DataSource = connector.Select("*", "Students");
			statusStripCountLabel.Text = $"Количество студентов: {dgvStudents.RowCount - 1}";
		}

		private void MainForm_Load(object sender, EventArgs e)
		{

		}

		private void dgvDisciplines_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		//private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//	int i = tabControl.SelectedIndex;
		//	Query query = queries[i];
		//	tables[i].DataSource = connector.Select(queries[i].Columns, queries[i].Tables, queries[i].Condition, queries[i].GroupBy);
		//	statusStripCountLabel.Text = $"{status_messages[i]} {tables[i].RowCount - 1}";
		//}
		//6
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = tabControl.SelectedIndex;
			Query query = queries[i];
			tables[i].DataSource = connector.Select(query.Columns, query.Tables, query.Condition, query.GroupBy);
			statusStripCountLabel.Text = $"{status_messages[i]} {tables[i].RowCount - 1}";
		}
		//
	}
}
