using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using ImageViewer;
using System.Windows.Media;

namespace ImageViewer
{
	class Interface
	{
		public List<Button> buttons;
		public List<TextBox> textBoxes;
		public List<Label> labels;
		public List<RowDefinition> rows;

		public enum TextBoxType
		{
			FullPathTextBox, DirectoryNameTextBox, KeyTextBox
		}
		public enum ButtonType
		{
			SelectButton, DeleteKeyButton
		}
		public enum LabelType
		{
			KeyLabel, OrLabel
		}

		public Interface()
		{
			buttons = new List<Button>();
			textBoxes = new List<TextBox>();
			labels = new List<Label>();
			rows = new List<RowDefinition>();
		}

		public void AddRow(RowDefinition row)
		{
			rows.Add(row);
		}

		public void AddButton(Button btn)
		{
			buttons.Add(btn);
		}

		public void AddLabel(Label lbl)
		{
			labels.Add(lbl);
		}

		public void AddTextBox(TextBox txtBox)
		{
			textBoxes.Add(txtBox);
		}

		public Label CreateLabel(string name, LabelType type)
		{
			Label lbl = new Label();
			lbl.Name = name;

			if(type == LabelType.KeyLabel)
			{
				lbl.Content = "Key:";
				lbl.HorizontalAlignment = HorizontalAlignment.Left;
				lbl.VerticalAlignment = VerticalAlignment.Top;
				lbl.Foreground = new SolidColorBrush(Colors.White);
				return lbl;
			}

			if(type == LabelType.OrLabel)
			{
				lbl.Content = "or";
				lbl.HorizontalAlignment = HorizontalAlignment.Left;
				lbl.VerticalAlignment = VerticalAlignment.Top;
				lbl.Foreground = new SolidColorBrush(Colors.White);
				return lbl;
			}

			return lbl;
		}

		public Button CreateButton(string name, ButtonType type)
		{
			Button btn = new Button();
			btn.Name = name;

			if (type == ButtonType.SelectButton)
			{
				btn.Content = "Select";
				btn.VerticalAlignment = VerticalAlignment.Top;
				btn.HorizontalAlignment = HorizontalAlignment.Left;
				btn.Foreground = new SolidColorBrush(Colors.Gray);
				btn.Height = 22;
				btn.BorderBrush = null;
				btn.Width = 35;
				btn.Background = new SolidColorBrush(Colors.White);
				return btn;
			}

			if(type == ButtonType.DeleteKeyButton)
			{
				btn.Content = "Delete key";
				btn.VerticalAlignment = VerticalAlignment.Top;
				btn.HorizontalAlignment = HorizontalAlignment.Left;
				return btn;
			}

			return btn;
		}

		public TextBox CreateTextBox(string name, TextBoxType type)
		{
			TextBox txtBox = new TextBox();
			txtBox.Name = name;

			if (type == TextBoxType.FullPathTextBox)
			{
				txtBox.IsReadOnly = false;
				txtBox.HorizontalAlignment = HorizontalAlignment.Left;
				txtBox.VerticalAlignment = VerticalAlignment.Top;
				txtBox.Width = 144;
				txtBox.Height = 20;
				return txtBox;
			}

			if(type == TextBoxType.DirectoryNameTextBox)
			{
				txtBox.IsReadOnly = false;
				txtBox.HorizontalAlignment = HorizontalAlignment.Left;
				txtBox.VerticalAlignment = VerticalAlignment.Top;
				txtBox.Width = 100;
				return txtBox;
			}

			if(type == TextBoxType.KeyTextBox)
			{
				txtBox.IsReadOnly = true;
				txtBox.HorizontalAlignment = HorizontalAlignment.Left;
				txtBox.VerticalAlignment = VerticalAlignment.Top;
				txtBox.Width = 40;

				return txtBox;
			}

			return txtBox;
		}

	}
}