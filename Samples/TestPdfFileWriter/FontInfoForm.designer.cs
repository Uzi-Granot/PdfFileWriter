namespace TestPdfFileWriter
{
	partial class FontInfoForm
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
			this.ViewButton = new System.Windows.Forms.Button();
			this.ExitButton = new System.Windows.Forms.Button();
			this.ButtonsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// ButtonsGroupBox
			// 
			this.ButtonsGroupBox.Controls.Add(this.ViewButton);
			this.ButtonsGroupBox.Controls.Add(this.ExitButton);
			this.ButtonsGroupBox.Location = new System.Drawing.Point(295, 381);
			this.ButtonsGroupBox.Name = "ButtonsGroupBox";
			this.ButtonsGroupBox.Size = new System.Drawing.Size(255, 71);
			this.ButtonsGroupBox.TabIndex = 0;
			this.ButtonsGroupBox.TabStop = false;
			// 
			// ViewButton
			// 
			this.ViewButton.Location = new System.Drawing.Point(24, 21);
			this.ViewButton.Name = "ViewButton";
			this.ViewButton.Size = new System.Drawing.Size(90, 36);
			this.ViewButton.TabIndex = 0;
			this.ViewButton.Text = "View";
			this.ViewButton.UseVisualStyleBackColor = true;
			this.ViewButton.Click += new System.EventHandler(this.OnView);
			// 
			// ExitButton
			// 
			this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ExitButton.Location = new System.Drawing.Point(141, 21);
			this.ExitButton.Name = "ExitButton";
			this.ExitButton.Size = new System.Drawing.Size(90, 36);
			this.ExitButton.TabIndex = 1;
			this.ExitButton.Text = "Exit";
			this.ExitButton.UseVisualStyleBackColor = true;
			// 
			// FontInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(844, 464);
			this.Controls.Add(this.ButtonsGroupBox);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "FontInfoForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "GlyphMetrics";
			this.Load += new System.EventHandler(this.OnLoad);
			this.Resize += new System.EventHandler(this.OnResize);
			this.ButtonsGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox ButtonsGroupBox;
		private System.Windows.Forms.Button ExitButton;
		private System.Windows.Forms.Button ViewButton;
	}
}