﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using System.Data.SqlClient;
using System.Configuration;

namespace AcademyDataSet
{
	public partial class MainForm : Form
	{
		readonly string CONNECTION_STRING = "";
		SqlConnection connection = null;
		System.Data.DataSet GroupsRelatedData = null;
		public MainForm()
		{
			InitializeComponent();
			CONNECTION_STRING = ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString;
			connection = new SqlConnection(CONNECTION_STRING);
			AllocConsole();
			Console.WriteLine(CONNECTION_STRING);
			//1) Создаем DataSet:
			GroupsRelatedData = new System.Data.DataSet();
			AddTable("Directions", "direction_id,direction_name");
			AddTable("Groups", "group_id,group_name,direction");
			AddRelation("GroupsDirections","Groups,direction", "Directions,direction_id");
			PrintGroups();
			//LoadGroupsRelatedData();
		}
		public void AddTable(string table, string columns)
		{
			//2) Добавляем табоицы в DataSet:
		//	const string dsTable_Directions = "Directions";
			//const string dst_col_direction_id = "direction_id";
			//const string dst_col_direction_name = "direction_name";
			//	2.1) Добавляем таблицу в DataSet:
			GroupsRelatedData.Tables.Add(table);

			//	2.2) Добавляем поля (столбики) в таблицу:
			string[] a_columns = columns.Split(',');
			for (int i = 0; i< a_columns.Length; i++)
			{
				GroupsRelatedData.Tables[table].Columns.Add(a_columns[i]);
			}

			//GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_id);
			//GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_name);
			//	2.3) Определяем, какое поле будет первичным ключем:
			GroupsRelatedData.Tables[table].PrimaryKey =
				 new DataColumn[] { GroupsRelatedData.Tables[table].Columns[0] };

			string cmd = $"SELECT {columns} FROM {table}";
			SqlDataAdapter adapter = new SqlDataAdapter(cmd, connection);
			adapter.Fill(GroupsRelatedData.Tables[table]);
			Print(table);
		}
		public void AddRelation(string relation_name, string child, string parent)
		{
			GroupsRelatedData.Relations.Add
				(
				relation_name,
				GroupsRelatedData.Tables[parent.Split(',')[0]].Columns[parent.Split(',')[1]],
				GroupsRelatedData.Tables[child.Split(',')[0]].Columns[child.Split(',')[1]]
				);
		}

		void LoadGroupsRelatedData()
		{
			//1) Создаем DataSet:
			GroupsRelatedData = new System.Data.DataSet();

			//2) Добавляем табоицы в DataSet:
			const string dsTable_Directions = "Directions";
			const string dst_col_direction_id = "direction_id";
			const string dst_col_direction_name = "direction_name";
			//	2.1) Добавляем таблицу в DataSet:
			GroupsRelatedData.Tables.Add(dsTable_Directions);
			//	2.2) Добавляем поля (столбики) в таблицу:
			GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_id, typeof(byte));
			GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_name, typeof(string));
			//	2.3) Определяем, какое поле будет первичным ключем:
			GroupsRelatedData.Tables[dsTable_Directions].PrimaryKey =
				 new DataColumn[] { GroupsRelatedData.Tables[dsTable_Directions].Columns[dst_col_direction_id] };

			const string dsTable_Groups = "Groups";
			const string dst_Groups_col_group_id = "group_id";
			const string dst_Groups_col_group_name = "group_name";
			const string dst_Groups_col_group_direction = "direction";
			GroupsRelatedData.Tables.Add(dsTable_Groups);
			GroupsRelatedData.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_id, typeof(int));
			GroupsRelatedData.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_name, typeof(string));
			GroupsRelatedData.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_direction, typeof(byte));
			GroupsRelatedData.Tables[dsTable_Groups].PrimaryKey =
				new DataColumn[] { GroupsRelatedData.Tables[dsTable_Groups].Columns[0] };

			//3) Строим связи между таблицами:
			GroupsRelatedData.Relations.Add
 				(
 					"GroupsDirections",
 					GroupsRelatedData.Tables[dsTable_Directions].Columns[dst_col_direction_id],        //Parent field - первичный ключ в другой таблице
					GroupsRelatedData.Tables[dsTable_Groups].Columns[dst_Groups_col_group_direction]//Child field - внешний ключ
				);

			//4) Загрузка данных в DataSet:
			string directionsCmd = "SELECT * FROM Directions";
			string groupsCmd = "SELECT * FROM Groups";
			SqlDataAdapter directionsAdapter = new SqlDataAdapter(directionsCmd, connection);
			SqlDataAdapter groupsAdapter = new SqlDataAdapter(groupsCmd, connection);

			directionsAdapter.Fill(GroupsRelatedData.Tables[dsTable_Directions]);
			groupsAdapter.Fill(GroupsRelatedData.Tables[dsTable_Groups]);

			Print("Directions");
			Print("Groups");
		}

			public void Print(string table)
		    {
			Console.WriteLine(table);
			Console.WriteLine("\n======================================================================\n");
			for (int i = 0; i < GroupsRelatedData.Tables[table].Columns.Count; i++)
				Console.Write(GroupsRelatedData.Tables[table].Columns[i].Caption + "\t");
			Console.WriteLine("\n----------------------------------------------------------------------\n");
			for (int i = 0; i < GroupsRelatedData.Tables[table].Rows.Count; i++)
			   {
			//	Console.Write(GroupsRelatedData.Tables[table].Rows[i] + ":\t");
				for (int j = 0; j < GroupsRelatedData.Tables[table].Columns.Count; j++)
				 {
					Console.Write(GroupsRelatedData.Tables[table].Rows[i][j] + "\t\t");
				 }
				Console.WriteLine();
			   }
			Console.WriteLine("\n======================================================================\n");
		}
		void PrintGroups()
		{
			Console.WriteLine("\n======================================================================\n");
			string table = "Groups";
			for (int i = 0; i< GroupsRelatedData.Tables[table].Rows.Count; i++)
			{
				for (int j = 0; j<GroupsRelatedData.Tables[table].Columns.Count; j++)
				{
					Console.Write(GroupsRelatedData.Tables[table].Rows[i][j] + "\t");
					
				}
				Console.WriteLine(GroupsRelatedData.Tables[table].Rows[i].GetParentRow("GroupsDirections")["direction_name"]);
				Console.WriteLine();
			}

			Console.WriteLine("\n======================================================================\n");
		}
		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();
		[DllImport("kernel32.dll")]
		public static extern bool FreeConsole();
	}
}