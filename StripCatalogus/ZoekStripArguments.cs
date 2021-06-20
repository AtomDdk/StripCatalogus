using Domeinlaag.Exceptions;
using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag
{
    public class ZoekStripArguments
    {
        private bool _FilterOpAuteur = false;
        public bool FilterOpAuteur
        {
            get => _FilterOpAuteur; 
            set
            {
                if (value == true)
                    throw new ZoekStripArgumentsException("FilterOpAuteur kan enkel op false gezet worden."); _FilterOpAuteur = value;
            }
        }
        private Auteur _Auteur;
        /// <summary>
        /// stel in op welke auteur er gefilterd moet worden.
        /// Om deze actie ongedaan te maken zet je FilterOpAuteur terug op false.
        /// </summary>
        public Auteur Auteur
        {
            get { return _Auteur; }
            set
            {
                if (value == null)
                {
                    _Auteur = null;
                    _FilterOpAuteur = false;
                }
                else
                {
                    _Auteur = value;
                    _FilterOpAuteur = true;
                }
            }
        }
        private bool _FilterOpUitgeverij= false;
        public bool FilterOpUitgeverij
        {
            get => _FilterOpUitgeverij;
            set
            {
                if (value == true)
                    throw new ZoekStripArgumentsException("FilterOpUitgeverij kan enkel op false gezet worden."); _FilterOpUitgeverij = value;
            }
        }
        private Uitgeverij _Uitgeverij;
        /// <summary>
        /// stel in op welke Uitgeverij er gefilterd moet worden.
        /// Om deze actie ongedaan te maken zet je FilterOpUitgeverij terug op false.
        /// </summary>
        public Uitgeverij Uitgeverij
        {
            get { return _Uitgeverij; }
            set
            {
                if (value == null)
                {
                    _Uitgeverij = null;
                    _FilterOpUitgeverij = false;
                }
                else
                {
                    _Uitgeverij = value;
                    _FilterOpUitgeverij = true;
                }
            }
        }
        private bool _FilterOpReeks= false;
        public bool FilterOpReeks
        {
            get => _FilterOpReeks;
            set
            {
                if (value == true)
                    throw new ZoekStripArgumentsException("FilterOpReeks kan enkel op false gezet worden."); _FilterOpReeks= value;
            }
        }
        private Reeks _Reeks;
        /// <summary>
        /// stel in op welke reeks er gefilterd moet worden.
        /// Om deze actie ongedaan te maken zet je FilterOpReeks terug op false.
        /// </summary>
        public Reeks Reeks
        {
            get { return _Reeks; }
            set
            {
                _Reeks = value;
                _FilterOpReeks = true;
            }
        }
        private bool _FilterOpReeksNummer = false;
        public bool FilterOpReeksNummer
        {
            get => _FilterOpReeksNummer;
            set
            {
                if (value == true)
                    throw new ZoekStripArgumentsException("FilterOpReeksNummer kan enkel op false gezet worden."); _FilterOpReeksNummer = value;
            }
        }
        private int _ReeksNummer;
        /// <summary>
        /// stel in op welk reeksNummer er gefilterd moet worden.
        /// Om deze actie ongedaan te maken zet je FilterOpReeksNummer terug op false.
        /// </summary>
        public int? ReeksNummer
        {
            get { return _ReeksNummer; }
            set
            {
                if (value == null)
                {
                    _ReeksNummer = 0;
                    FilterOpReeksNummer = false;
                }
                else
                {
                    _ReeksNummer = (int)value;
                    _FilterOpReeksNummer = true;
                }
            }
        }
        private bool _FilterOpTitel = false;
        public bool FilterOpTitel
        {
            get => _FilterOpTitel;
            set
            {
                if (value == true)
                    throw new ZoekStripArgumentsException("FilterOpTitel kan enkel op false gezet worden."); _FilterOpTitel = value;
            }
        }
        private string _Titel;

        /// <summary>
        /// stel in op welke Titel er gefilterd moet worden.
        /// Om deze actie ongedaan te maken zet je FilterOpTitel terug op false.
        /// </summary>
        public string Titel
        {
            get { return _Titel; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _Titel = null;
                    _FilterOpTitel= false;
                }
                else
                {
                    _Titel = value;
                    _FilterOpTitel = true;
                }
            }
        }

    }
}
