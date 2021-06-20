using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ImportEnExport;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImportEnExportTool
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Parser _parser;
        public MainWindowViewModel(Parser parser)
        {
            _parser = parser;
            BladerImportCommand = new RelayCommand(BladerImport);
            ImportCommand = new RelayCommand(Import);
            ExportCommand = new RelayCommand(Export);
        }
        /// <summary>
        /// het pad dat de parser volgt om het bestand in te lezen.
        /// </summary>
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                RaisePropertyChanged("FilePath");
            }
        }

        public RelayCommand BladerImportCommand { get; set; }
        /// <summary>
        /// Kiest een pad via de filebrowser en zet het in de textbox.
        /// </summary>
        public void BladerImport()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                FilePath = openFileDialog.FileName;
        }

        public RelayCommand ImportCommand { get; set; }
        /// <summary>
        /// Stuurt een filepath door naar de parser en zet de ingeladen en niet-ingeladen properties
        /// </summary>
        private void Import()
        {
            try
            {
                using (WaitCursor waitCursor = new WaitCursor())
                {
                    Report report = _parser.Import(FilePath);
                    IngeladenStrips = report.AantalIngeladenStrips;
                    NietIngeladenStrips = report.AantalNietIngeladenStrips;

                    using StreamWriter writer = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "errorLog.txt"));
                    writer.WriteLine(report.ErrorLog());
                    writer.Dispose();
                }
                MessageBox.Show("De import is gelukt!", "Succes", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden." + ex.Message, "Veel geluk", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private int _ingeladenStrips;
        public int IngeladenStrips
        {
            get { return _ingeladenStrips; }
            set
            {
                _ingeladenStrips = value;
                RaisePropertyChanged("IngeladenStrips");
            }
        }


        private int _nietIngeladenStrips;
        public int NietIngeladenStrips
        {
            get { return _nietIngeladenStrips; }
            set
            {
                _nietIngeladenStrips = value;
                RaisePropertyChanged("NietIngeladenStrips");
            }
        }

        public RelayCommand ExportCommand { get; set; }
        /// <summary>
        /// Opent een filebrowser en laat kiezen hoe het bestand noemt en waar het wordt opgeslagen.
        /// </summary>
        private void Export()
        {
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = ".json",
                FileName = "*.json",
                OverwritePrompt = true
            };

            try
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    _parser.Export(saveFileDialog.FileName);
                    MessageBox.Show("De export is gelukt!", "Succes", MessageBoxButton.OK);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden." + ex.Message, "Veel geluk", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
