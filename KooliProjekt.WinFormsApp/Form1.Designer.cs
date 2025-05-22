namespace KooliProjekt.WinFormsApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DoctorsGrid = new DataGridView();
            IdLabel = new Label();
            IdField = new TextBox();
            NameLabel = new Label();
            NameField = new TextBox();
            SpecializationLabel = new Label();
            SpecializationField = new TextBox();
            NewButton = new Button();
            SaveButton = new Button();
            DeleteButton = new Button();
            ((System.ComponentModel.ISupportInitialize)DoctorsGrid).BeginInit();
            SuspendLayout();

            // DoctorsGrid
            DoctorsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DoctorsGrid.Location = new Point(15, 15);
            DoctorsGrid.Margin = new Padding(4);
            DoctorsGrid.Name = "DoctorsGrid";
            DoctorsGrid.RowHeadersWidth = 51;
            DoctorsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DoctorsGrid.Size = new Size(625, 720);
            DoctorsGrid.TabIndex = 0;
            DoctorsGrid.SelectionChanged += DoctorsGrid_SelectionChanged;

            // IdLabel
            IdLabel.AutoSize = true;
            IdLabel.Location = new Point(662, 19);
            IdLabel.Margin = new Padding(4, 0, 4, 0);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(34, 25);
            IdLabel.TabIndex = 1;
            IdLabel.Text = "ID:";

            // IdField
            IdField.Location = new Point(788, 15);
            IdField.Margin = new Padding(4);
            IdField.Name = "IdField";
            IdField.ReadOnly = true;
            IdField.Size = new Size(336, 31);
            IdField.TabIndex = 2;

            // NameLabel
            NameLabel.AutoSize = true;
            NameLabel.Location = new Point(662, 69);
            NameLabel.Margin = new Padding(4, 0, 4, 0);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(63, 25);
            NameLabel.TabIndex = 3;
            NameLabel.Text = "Name:";

            // NameField
            NameField.Location = new Point(788, 65);
            NameField.Margin = new Padding(4);
            NameField.Name = "NameField";
            NameField.Size = new Size(336, 31);
            NameField.TabIndex = 4;

            // SpecializationLabel
            SpecializationLabel.AutoSize = true;
            SpecializationLabel.Location = new Point(662, 119);
            SpecializationLabel.Margin = new Padding(4, 0, 4, 0);
            SpecializationLabel.Name = "SpecializationLabel";
            SpecializationLabel.Size = new Size(123, 25);
            SpecializationLabel.TabIndex = 5;
            SpecializationLabel.Text = "Specialization:";

            // SpecializationField
            SpecializationField.Location = new Point(788, 115);
            SpecializationField.Margin = new Padding(4);
            SpecializationField.Name = "SpecializationField";
            SpecializationField.Size = new Size(336, 31);
            SpecializationField.TabIndex = 6;

            // NewButton
            NewButton.Location = new Point(788, 175);
            NewButton.Margin = new Padding(4);
            NewButton.Name = "NewButton";
            NewButton.Size = new Size(106, 44);
            NewButton.TabIndex = 7;
            NewButton.Text = "New";
            NewButton.UseVisualStyleBackColor = true;
            NewButton.Click += NewButton_Click;

            // SaveButton
            SaveButton.Location = new Point(912, 175);
            SaveButton.Margin = new Padding(4);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(106, 44);
            SaveButton.TabIndex = 8;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;

            // DeleteButton
            DeleteButton.Location = new Point(1038, 175);
            DeleteButton.Margin = new Padding(4);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(106, 44);
            DeleteButton.TabIndex = 9;
            DeleteButton.Text = "Delete";
            DeleteButton.UseVisualStyleBackColor = true;
            DeleteButton.Click += DeleteButton_Click;

            // Form1
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1165, 750);
            Controls.Add(DeleteButton);
            Controls.Add(SaveButton);
            Controls.Add(NewButton);
            Controls.Add(SpecializationField);
            Controls.Add(SpecializationLabel);
            Controls.Add(NameField);
            Controls.Add(NameLabel);
            Controls.Add(IdField);
            Controls.Add(IdLabel);
            Controls.Add(DoctorsGrid);
            Margin = new Padding(4);
            Name = "Form1";
            Text = "Doctors Management";
            ((System.ComponentModel.ISupportInitialize)DoctorsGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView DoctorsGrid;
        private Label IdLabel;
        private TextBox IdField;
        private Label NameLabel;
        private TextBox NameField;
        private Label SpecializationLabel;
        private TextBox SpecializationField;
        private Button NewButton;
        private Button SaveButton;
        private Button DeleteButton;
    }
}