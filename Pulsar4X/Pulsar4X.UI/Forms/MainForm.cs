﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Pulsar4X.UI.ViewModels;

namespace Pulsar4X.UI.Forms
{
    public partial class MainForm : Form
    {
        DockPanel m_oDockPanel;
        MenuViewModel VM;

        public MainForm()
        {
            InitializeComponent();

            // Create and Add docking Panel:
            m_oDockPanel = new DockPanel();
            m_oDockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            m_oDockPanel.Dock = DockStyle.Fill;
            m_oDockPanel.BackgroundImage = new Bitmap(UIConstants.PULSAR4X_LOGO);
            m_oDockPanel.BackgroundImageLayout = ImageLayout.Center;

            // set Mono only stuff:
            if (Helpers.UIController.Instance.IsRunningOnMono)
            {
                m_oDockPanel.SupportDeeplyNestedContent = false;
                m_oDockPanel.AllowEndUserDocking = false;
                m_oDockPanel.AllowEndUserNestedDocking = false;
            }

            m_oToolStripContainer.ContentPanel.Controls.Add(m_oDockPanel);

            // setup viewmodel:
            VM = new MenuViewModel();

            // bind menu items to game data:
            //this.Bind(c => c.Text, VM, d => d.GameDateTime);
            this.Text = "Pulsar4X - " + GameState.Instance.GameDateTime.ToString();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Setup Events:
            exitToolStripMenuItem.Click += new EventHandler(exitToolStripMenuItem_Click);
            aboutToolStripMenuItem.Click += new EventHandler(aboutToolStripMenuItem_Click);
            systemInformationToolStripMenuItem.Click += new EventHandler(systemInformationToolStripMenuItem_Click);
            m_oSystemViewToolStripButton.Click += new EventHandler(systemInformationToolStripMenuItem_Click);
            systemMapToolStripMenuItem.Click += new EventHandler(systemMapToolStripMenuItem_Click);
            m_oSystemMapToolStripButton.Click += new EventHandler(systemMapToolStripMenuItem_Click);
            commanderNamesToolStripMenuItem.Click += new EventHandler(commanderNamesToolStripMenuItem_Click);
            economicsToolStripMenuItem.Click += new EventHandler(economicsToolStripMenuItem_Click);
            shipsToolStripMenuItem.Click += new EventHandler(shipsToolStripMenuItem_Click);
            classDesignToolStripMenuItem.Click += new EventHandler(classDesignToolStripMenuItem_Click);

            taskGroupsToolStripMenuItem.Click += new EventHandler(taskGroupsToolStripMenuItem_Click);

            sMOnToolStripMenuItem.Click += new EventHandler(sMOnToolStripMenuItem_Click);
            sMOffToolStripMenuItem.Click += new EventHandler(sMOffToolStripMenuItem_Click);

            // Set up the proportion of the windows used by docing panels at different locations:
            m_oDockPanel.DockBottomPortion = 0.2f;
            m_oDockPanel.DockLeftPortion = 0.2f;
            m_oDockPanel.DockRightPortion = 0.2f;
            m_oDockPanel.ActiveDocumentChanged += new EventHandler(m_oDockPanel_ActiveDocumentChanged);

            /// <summary>
            /// Who doesn't like spaghetti references?
            /// </summary>
            Helpers.UIController.Instance.SystemMap.MainFormReference = this;
            Helpers.UIController.Instance.Economics.MainFormReference = this;
            Helpers.UIController.Instance.Economics.SystemMapReference = Helpers.UIController.Instance.SystemMap;
        }

        #region MenuAndToolStripEvents

        void classDesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.ClassDesign.ShowAllPanels(m_oDockPanel);
        }

        void shipsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.Ships.ShowAllPanels(m_oDockPanel);
        }

        void economicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.Economics.ShowAllPanels(m_oDockPanel);
        }

        void commanderNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialogs.CommanderNameThemesDialog oForm = new Dialogs.CommanderNameThemesDialog();
            oForm.ShowDialog();
        }

        void m_oDockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.DockPanelActiveDocumentChanged(m_oDockPanel);
        }

        void sMOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.SMOff();
        }

        void sMOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.SMOn();
        }

        void systemMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.SystemMap.ShowAllPanels(m_oDockPanel);
        }

        void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialogs.AboutBox frmAboutBox = new Dialogs.AboutBox();
            frmAboutBox.ShowDialog();
        }

        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void systemInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.SystemGenAndDisplay.ShowAllPanels(m_oDockPanel);
        }

        private void taskGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.TaskGroup.ShowAllPanels(m_oDockPanel);
        }

        private void createResearchProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.ComponentRP.Popup();
        }

        private void fastOOBCreationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.UIController.Instance.FastOOBScreen.Popup();
        }

        #endregion
    }
}
