﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarRental.WindowsApp.Shared
{
    public static class DataGridViewExtensions
    {
        public static void ConfigureZebraGrid(this DataGridView grid)
        {
            Font font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);

            DataGridViewCellStyle darkRow = new DataGridViewCellStyle
            {
                BackColor = Color.LightGray,
                Font = font,
                ForeColor = Color.Black,
                SelectionBackColor = Color.LightYellow,
                SelectionForeColor = Color.Black
            };

            grid.AlternatingRowsDefaultCellStyle = darkRow;

            DataGridViewCellStyle lightRow = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = font,
                SelectionBackColor = Color.LightYellow,
                SelectionForeColor = Color.Black
            };

            grid.RowsDefaultCellStyle = lightRow;
        }

        public static void ConfigureReadOnlyGrid(this DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;

            grid.BorderStyle = BorderStyle.None;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            grid.MultiSelect = false;
            grid.ReadOnly = true;

            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoGenerateColumns = false;

            grid.AllowUserToResizeRows = false;

            grid.RowsAdded += (sender, e) =>
            {
                grid.ClearSelection();
            };

            grid.RowsRemoved += (sender, e) =>
            {
                grid.ClearSelection();
            };
        }

        public static T SelectId<T>(this DataGridView grid)
        {
            const int firstLine = 0, firstColumn = 0;
            if (grid.SelectedRows.Count == 0)
                return default(T);

            object value = grid.SelectedRows[firstLine].Cells[firstColumn].Value;

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
