namespace TestPdfFileWriter
{
	partial class DrawGlyphForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ButtonsGroupBox = new System.Windows.Forms.GroupBox();
			this.FormatComboBox = new System.Windows.Forms.ComboBox();
			this.FillColorButton = new System.Windows.Forms.Button();
			this.OutlineColorButton = new System.Windows.Forms.Button();
			this.ExitButton = new System.Windows.Forms.Button();
			this.ButtonsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// ButtonsGroupBox
			// 
			this.ButtonsGroupBox.Controls.Add(this.FormatComboBox);
			this.ButtonsGroupBox.Controls.Add(this.FillColorButton);
			this.ButtonsGroupBox.Controls.Add(this.OutlineColorButton);
			this.ButtonsGroupBox.Controls.Add(this.ExitButton);
			this.ButtonsGroupBox.Location = new System.Drawing.Point(12, 398);
			this.ButtonsGroupBox.Name = "ButtonsGroupBox";
			this.ButtonsGroupBox.Size = new System.Drawing.Size(495, 67);
			this.ButtonsGroupBox.TabIndex = 0;
			this.ButtonsGroupBox.TabStop = false;
			// 
			// FormatComboBox
			// 
			this.FormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FormatComboBox.FormattingEnabled = true;
			this.FormatComboBox.Items.AddRange(new object[] {
            "Fill",
            "Outline",
            "Fill+Outline"});
			this.FormatComboBox.Location = new System.Drawing.Point(35, 21);
			this.FormatComboBox.Name = "FormatComboBox";
			this.FormatComboBox.Size = new System.Drawing.Size(121, 24);
			this.FormatComboBox.TabIndex = 0;
			this.FormatComboBox.SelectedIndexChanged += new System.EventHandler(this.OnOutlineChanged);
			// 
			// FillColorButton
			// 
			this.FillColorButton.Location = new System.Drawing.Point(178, 14);
			this.FillColorButton.Name = "FillColorButton";
			this.FillColorButton.Size = new System.Drawing.Size(84, 36);
			this.FillColorButton.TabIndex = 1;
			this.FillColorButton.Text = "Fill";
			this.FillColorButton.UseVisualStyleBackColor = true;
			this.FillColorButton.Click += new System.EventHandler(this.OnFillColor);
			// 
			// OutlineColorButton
			// 
			this.OutlineColorButton.Location = new System.Drawing.Point(284, 14);
			this.OutlineColorButton.Name = "OutlineColouButton";
			this.OutlineColorButton.Size = new System.Drawing.Size(84, 36);
			this.OutlineColorButton.TabIndex = 2;
			this.OutlineColorButton.Text = "Outline";
			this.OutlineColorButton.UseVisualStyleBackColor = true;
			this.OutlineColorButton.Click += new System.EventHandler(this.OnFillColor);
			// 
			// ExitButton
			// 
			this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ExitButton.Location = new System.Drawing.Point(390, 14);
			this.ExitButton.Name = "ExitButton";
			this.ExitButton.Size = new System.Drawing.Size(70, 36);
			this.ExitButton.TabIndex = 3;
			this.ExitButton.Text = "Exit";
			this.ExitButton.UseVisualStyleBackColor = true;
			// 
			// DrawGlyphForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ExitButton;
			this.ClientSize = new System.Drawing.Size(519, 477);
			this.Controls.Add(this.ButtonsGroupBox);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "DrawGlyphForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Draw Glyph";
			this.Load += new System.EventHandler(this.OnLoad);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
			this.Resize += new System.EventHandler(this.OnResize);
			this.ButtonsGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox ButtonsGroupBox;
		private System.Windows.Forms.Button ExitButton;
		private System.Windows.Forms.Button FillColorButton;
		private System.Windows.Forms.Button OutlineColorButton;
		private System.Windows.Forms.ComboBox FormatComboBox;
	}
}