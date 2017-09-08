using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using ImageViewer;

namespace ImageViewer
{
	[Serializable]
	class ControlPanel
	{
		const string DELETE_KEY_BUTTON_NAME = "DeleteButton";
		const string KEY_LABEL_NAME = "KeyLabel";
		const string KEY_TEXTBOX_NAME = "KeyTextBox";
		const string DIRECTORY_NAME_TEXTBOX_NAME = "DirectoryNameTextBox";
		const string CONTROL_ROW_NAME = "ControlRow";
		const string SUBDIRECTORY_LABEL_NAME = "SubdirectoryLabel";

		public enum TextBoxType
		{
			DirectoryNameTextBox, KeyTextBox
		}

		public enum LabelType
		{
			KeyLabel, SubdirectoryLabel
		}

		public Button DeleteButton { get; set; }
		public Label KeyLabel { get; set; }
		public Label SubdirectoryLabel { get; set; }
		public TextBox KeyTextBox { get; set; }
		public TextBox DirectoryNameTextBox { get; set; }
		public RowDefinition ControlRow { get; set; }
		public int Index { get; private set; }

		public ControlPanel(int index)
		{
			Index = index;
			DeleteButton = CreateButton();
			ControlRow = CreateRowDefinition();
			KeyTextBox = CreateTextBox(TextBoxType.KeyTextBox);
			DirectoryNameTextBox = CreateTextBox(TextBoxType.DirectoryNameTextBox);
			KeyLabel = CreateLabel(LabelType.KeyLabel);
			SubdirectoryLabel = CreateLabel(LabelType.SubdirectoryLabel);
		}

		public int GetIndex(IFrameworkInputElement element)
		{
			int index = Convert.ToInt32(element.Name.Remove(0, element.Name.Length - 1));
			return index;
		}

		public void ChangeIndex(int index)
		{
			if(index > 0)
			{
				Index = index;
				DeleteButton.Name = String.Concat(DELETE_KEY_BUTTON_NAME, index);
				ControlRow.Name = String.Concat(CONTROL_ROW_NAME, index);
				KeyTextBox.Name = String.Concat(KEY_TEXTBOX_NAME, index);
				DirectoryNameTextBox.Name = String.Concat(DIRECTORY_NAME_TEXTBOX_NAME, index);
				KeyLabel.Name = String.Concat(KEY_LABEL_NAME, index);
				SubdirectoryLabel.Name = String.Concat(SUBDIRECTORY_LABEL_NAME, index);

				Grid.SetRow(DeleteButton, Index);
				Grid.SetRow(KeyTextBox, Index);
				Grid.SetRow(DirectoryNameTextBox, Index);
				Grid.SetRow(KeyLabel, Index);
				Grid.SetRow(SubdirectoryLabel, Index);
			}
		}

		public Button CreateButton()
		{
			Button btn = new Button
			{
				Name = String.Concat(DELETE_KEY_BUTTON_NAME, Index),
				Margin = new Thickness(0, 5, 25, 0)
			};

			btn.VerticalAlignment = VerticalAlignment.Top;
			btn.HorizontalAlignment = HorizontalAlignment.Right;
			btn.Content = "Delete";
			Grid.SetRow(btn, Index);

			return btn;
		}

		public TextBox CreateTextBox(TextBoxType type)
		{
			TextBox txtBox = new TextBox
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top
			};

			Grid.SetRow(txtBox, Index);

			if (type == TextBoxType.DirectoryNameTextBox)
			{
				txtBox.Name = String.Concat(DIRECTORY_NAME_TEXTBOX_NAME, Index);
				txtBox.IsReadOnly = false;
				txtBox.Width = 200;
				txtBox.Margin = new Thickness(105, 30, 5, 0);
			}

			if (type == TextBoxType.KeyTextBox)
			{
				txtBox.Name = String.Concat(KEY_TEXTBOX_NAME, Index);
				txtBox.IsReadOnly = true;
				txtBox.Width = 40;
				txtBox.Margin = new Thickness(35, 4, 0, 0);
			}

			return txtBox;
		}

		public Label CreateLabel(LabelType type)
		{
			Label lbl = new Label
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				Foreground = new SolidColorBrush(Colors.LightGray)
			};

			Grid.SetRow(lbl, Index);

			if (type == LabelType.KeyLabel)
			{
				lbl.Name = String.Concat(KEY_LABEL_NAME, Index);
				lbl.Content = "Key:";
				lbl.Margin = new Thickness(5, 0, 0, 0);
			}

			if(type == LabelType.SubdirectoryLabel)
			{
				lbl.Name = String.Concat(DIRECTORY_NAME_TEXTBOX_NAME, Index);
				lbl.Content = "Subfolder name:";
				lbl.Margin = new Thickness(5, 28, 0, 0);
			}
			return lbl;
		}

		public RowDefinition CreateRowDefinition()
		{
			RowDefinition rd = new RowDefinition();
			rd.Height = new GridLength(65, GridUnitType.Pixel);
			rd.Name = String.Concat(CONTROL_ROW_NAME, Index);
			return rd;
		}

		public void SetKey(string key)
		{
			KeyTextBox.Text = key;
		}

		public void SetDirectory(string directoryName)
		{
			DirectoryNameTextBox.Text = directoryName;
		}
	}
}
