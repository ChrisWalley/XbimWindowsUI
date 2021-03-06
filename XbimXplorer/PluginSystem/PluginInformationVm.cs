﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NuGet;
using XbimXplorer.Annotations;

namespace XbimXplorer.PluginSystem
{
    public class PluginInformationVm : INotifyPropertyChanged
    {
        private readonly PluginInformation _model;

        public PluginInformationVm(PluginInformation model)
        {
            _model = model;
        }

        public bool CanDownload
        {
            get
            {
                if (string.IsNullOrEmpty(AvailableVersion))
                    return false;
                return AvailableVersion != InstalledVersion;
            }
        }

        public string DownloadCaption
        {
            get
            {
                // when button is grayed 
                if (!CanDownload)
                    return "Download";

                // if can download and not installed 
                if (string.IsNullOrWhiteSpace(InstalledVersion))
                    return "Download";

                // if installed
                var remoteVersion = new SemanticVersion(AvailableVersion);
                var localVersion = new SemanticVersion(InstalledVersion);

                return remoteVersion > localVersion 
                    ? "Update" 
                    : "Replace";
            }
        }

        public bool IsInstalled => !string.IsNullOrWhiteSpace(InstalledVersion);

        public bool IsInstalledAndNotLoaded => !string.IsNullOrWhiteSpace(InstalledVersion) && string.IsNullOrWhiteSpace(LoadedVersion);

        public string PluginId => _model?.PluginId;

        internal void RemoveInstallation()
        {
            if (_model == null)
                return;
            _model.DeleteFromDisk();
            OnPropertyChanged("");
        }

        public bool CanLoad => IsInstalled && string.IsNullOrEmpty(LoadedVersion);

        public Visibility VisibleWhenInstalled => IsInstalled 
            ? Visibility.Visible 
            : Visibility.Hidden;

        public string EnableToggleCaption => _model?.Config?.OnStartup == PluginConfiguration.StartupBehaviour.Enabled 
            ? "Disable" 
            : "Enable";

        public string AvailableVersion => _model?.AvailableVersion;

        public string InstalledVersion => _model?.InstalledVersion;

        public string LoadedVersion => _model?.LoadedVersion;

        public string Startup => _model?.Config?.OnStartup.ToString();

        public void ExtractPlugin(DirectoryInfo pluginsDirectory)
        {
            if (_model == null)
                return;
            
            _model.ExtractPlugin(pluginsDirectory);
            OnPropertyChanged("");
        }

        public string Action
        {
            get
            {
                if (LoadedVersion != "" && InstalledVersion != LoadedVersion)
                    return "Restart required.";
                return "";
            }
        }

        /// <returns>True if plugin is completely loaded. False if not, for any reason.</returns>
        public bool Load()
        {
            if (_model == null)
                return false;

            var l = _model.Load();
            OnPropertyChanged("");
            return l;
        }

        public void ToggleEnabled()
        {
            if (_model == null)
                return;

            _model.ToggleEnabled();
            OnPropertyChanged("");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
