namespace TestPdfFileWriter
{
	partial class EnumFontFamilies
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
			this.ExitButton = new System.Windows.Forms.Button();
			this.ViewButton = new System.Windows.Forms.Button();
			this.ButtonsGroupBox = new System.Windows.Forms.GroupBox();
			this.ButtonsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// ExitButton
			// 
			this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ExitButton.Location = new System.Drawing.Point(163, 19);
			this.ExitButton.Name = "ExitButton";
			this.ExitButton.Size = new System.Drawing.Size(111, 41);
			this.ExitButton.TabIndex = 1;
			this.ExitButton.Text = "Exit";
			this.ExitButton.UseVisualStyleBackColor = true;
			this.ExitButton.Click += new System.EventHandler(this.OnExit);
			// 
			// ViewButton
			// 
			this.ViewButton.Location = new System.Drawing.Point(30, 19);
			this.ViewButton.Name = "ViewButton";
			this.ViewButton.Size = new System.Drawing.Size(111, 41);
			this.ViewButton.TabIndex = 0;
			this.ViewButton.Text = "View";
			this.ViewButton.UseVisualStyleBackColor = true;
			this.ViewButton.Click += new System.EventHandler(this.OnView);
			// 
			// ButtonsGroupBox
			// 
			this.ButtonsGroupBox.Controls.Add(this.ViewButton);
			this.ButtonsGroupBox.Controls.Add(this.ExitButton);
			this.ButtonsGroupBox.Location = new System.Drawing.Point(236, 491);
			this.ButtonsGroupBox.Name = "ButtonsGroupBox";
			this.ButtonsGroupBox.Size = new System.Drawing.Size(304, 69);
			this.ButtonsGroupBox.TabIndex = 0;
			this.ButtonsGroupBox.TabStop = false;
			// 
			// EnumFontFamilies
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ExitButton;
			this.ClientSize = new System.Drawing.Size(776, 563);
			this.Controls.Add(this.ButtonsGroupBox);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.Name = "EnumFontFamilies";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Enumerate Font Families";
			this.Load += new System.EventHandler(this.OnLoad);
			this.Resize += new System.EventHandler(this.OnResize);
			this.ButtonsGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button ExitButton;
		private System.Windows.Forms.Button ViewButton;
		private System.Windows.Forms.GroupBox ButtonsGroupBox;
	}
}

