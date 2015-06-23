namespace CSharpProjekt
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        delegate void onClose();

        private void onFormClose()
        {
            imgForm.Dispose();
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //rewriting the xml file simulating our DataBase
            DataBaseInterface.Instance.commit();
            //decided to not use this, if we try to close the imgForm we get some kind of an infinite loop
            //probably because imgForm.Dispose calls Form1 to set the variable to null.
            /*if (imgForm != null)
            {
                imgForm.Invoke(new onClose(onFormClose));
            }*/
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
            this.tabbed_views = new System.Windows.Forms.TabControl();
            this.hot_tab_page = new System.Windows.Forms.TabPage();
            this.hot_flow_layout = new System.Windows.Forms.FlowLayoutPanel();
            this.newest_tab_page = new System.Windows.Forms.TabPage();
            this.newest_flow_layout = new System.Windows.Forms.FlowLayoutPanel();
            this.tag_search_tab_page = new System.Windows.Forms.TabPage();
            this.search_flow_layout = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.gallery_tab_page = new System.Windows.Forms.TabPage();
            this.local_flow_layout = new System.Windows.Forms.FlowLayoutPanel();
            this.tabbed_views.SuspendLayout();
            this.hot_tab_page.SuspendLayout();
            this.newest_tab_page.SuspendLayout();
            this.tag_search_tab_page.SuspendLayout();
            this.gallery_tab_page.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabbed_views
            // 
            this.tabbed_views.Controls.Add(this.hot_tab_page);
            this.tabbed_views.Controls.Add(this.newest_tab_page);
            this.tabbed_views.Controls.Add(this.tag_search_tab_page);
            this.tabbed_views.Controls.Add(this.gallery_tab_page);
            this.tabbed_views.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbed_views.Location = new System.Drawing.Point(0, 0);
            this.tabbed_views.Name = "tabbed_views";
            this.tabbed_views.SelectedIndex = 0;
            this.tabbed_views.Size = new System.Drawing.Size(857, 561);
            this.tabbed_views.TabIndex = 0;
            // 
            // hot_tab_page
            // 
            this.hot_tab_page.Controls.Add(this.hot_flow_layout);
            this.hot_tab_page.Location = new System.Drawing.Point(4, 22);
            this.hot_tab_page.Name = "hot_tab_page";
            this.hot_tab_page.Padding = new System.Windows.Forms.Padding(3);
            this.hot_tab_page.Size = new System.Drawing.Size(849, 535);
            this.hot_tab_page.TabIndex = 0;
            this.hot_tab_page.Text = "Hot Deviations";
            this.hot_tab_page.UseVisualStyleBackColor = true;
            // 
            // hot_flow_layout
            // 
            this.hot_flow_layout.AutoScroll = true;
            this.hot_flow_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hot_flow_layout.Location = new System.Drawing.Point(3, 3);
            this.hot_flow_layout.Name = "hot_flow_layout";
            this.hot_flow_layout.Size = new System.Drawing.Size(843, 529);
            this.hot_flow_layout.TabIndex = 0;
            // 
            // newest_tab_page
            // 
            this.newest_tab_page.Controls.Add(this.newest_flow_layout);
            this.newest_tab_page.Location = new System.Drawing.Point(4, 22);
            this.newest_tab_page.Name = "newest_tab_page";
            this.newest_tab_page.Padding = new System.Windows.Forms.Padding(3);
            this.newest_tab_page.Size = new System.Drawing.Size(849, 535);
            this.newest_tab_page.TabIndex = 1;
            this.newest_tab_page.Text = "Newest";
            this.newest_tab_page.UseVisualStyleBackColor = true;
            // 
            // newest_flow_layout
            // 
            this.newest_flow_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newest_flow_layout.Location = new System.Drawing.Point(3, 3);
            this.newest_flow_layout.Name = "newest_flow_layout";
            this.newest_flow_layout.Size = new System.Drawing.Size(843, 529);
            this.newest_flow_layout.TabIndex = 0;
            // 
            // tag_search_tab_page
            // 
            this.tag_search_tab_page.Controls.Add(this.search_flow_layout);
            this.tag_search_tab_page.Controls.Add(this.button1);
            this.tag_search_tab_page.Controls.Add(this.textBox1);
            this.tag_search_tab_page.Location = new System.Drawing.Point(4, 22);
            this.tag_search_tab_page.Name = "tag_search_tab_page";
            this.tag_search_tab_page.Padding = new System.Windows.Forms.Padding(3);
            this.tag_search_tab_page.Size = new System.Drawing.Size(849, 535);
            this.tag_search_tab_page.TabIndex = 2;
            this.tag_search_tab_page.Text = "Tag Search";
            this.tag_search_tab_page.UseVisualStyleBackColor = true;
            // 
            // search_flow_layout
            // 
            this.search_flow_layout.Location = new System.Drawing.Point(8, 32);
            this.search_flow_layout.Name = "search_flow_layout";
            this.search_flow_layout.Size = new System.Drawing.Size(833, 598);
            this.search_flow_layout.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(511, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(189, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(497, 20);
            this.textBox1.TabIndex = 0;
            // 
            // gallery_tab_page
            // 
            this.gallery_tab_page.Controls.Add(this.local_flow_layout);
            this.gallery_tab_page.Location = new System.Drawing.Point(4, 22);
            this.gallery_tab_page.Name = "gallery_tab_page";
            this.gallery_tab_page.Padding = new System.Windows.Forms.Padding(3);
            this.gallery_tab_page.Size = new System.Drawing.Size(849, 535);
            this.gallery_tab_page.TabIndex = 3;
            this.gallery_tab_page.Text = "Local Gallery";
            this.gallery_tab_page.UseVisualStyleBackColor = true;
            // 
            // local_flow_layout
            // 
            this.local_flow_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.local_flow_layout.Location = new System.Drawing.Point(3, 3);
            this.local_flow_layout.Name = "local_flow_layout";
            this.local_flow_layout.Size = new System.Drawing.Size(843, 529);
            this.local_flow_layout.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 561);
            this.Controls.Add(this.tabbed_views);
            this.Name = "Form1";
            this.Text = "Deviantart Browser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabbed_views.ResumeLayout(false);
            this.hot_tab_page.ResumeLayout(false);
            this.newest_tab_page.ResumeLayout(false);
            this.tag_search_tab_page.ResumeLayout(false);
            this.tag_search_tab_page.PerformLayout();
            this.gallery_tab_page.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabbed_views;
        private System.Windows.Forms.TabPage hot_tab_page;
        private System.Windows.Forms.TabPage newest_tab_page;
        private System.Windows.Forms.TabPage tag_search_tab_page;
        private System.Windows.Forms.TabPage gallery_tab_page;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.FlowLayoutPanel hot_flow_layout;
        private System.Windows.Forms.FlowLayoutPanel newest_flow_layout;
        private System.Windows.Forms.FlowLayoutPanel search_flow_layout;
        private System.Windows.Forms.FlowLayoutPanel local_flow_layout;


    }
}

