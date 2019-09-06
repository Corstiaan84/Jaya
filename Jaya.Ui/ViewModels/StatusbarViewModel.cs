﻿using Jaya.Ui.Base;
using Jaya.Ui.Models;
using Jaya.Ui.Services;

namespace Jaya.Ui.ViewModels
{
    public class StatusbarViewModel: ViewModelBase
    {
        readonly Subscription<DirectoryChangedEventArgs> _onDirectoryChanged;
        readonly ConfigurationService _configService;
        ProviderServiceBase _service;
        ProviderModel _provider;
        DirectoryModel _directory;

        public StatusbarViewModel()
        {
            _onDirectoryChanged = EventAggregator.Subscribe<DirectoryChangedEventArgs>(DirectoryChanged);
            _configService = GetService<ConfigurationService>();
        }

        ~StatusbarViewModel()
        {
            EventAggregator.UnSubscribe(_onDirectoryChanged);
        }

        public PaneConfigModel PaneConfig => _configService.PaneConfiguration;

        public long Count
        {
            get => Get<long>();
            private set => Set(value);
        }

        async void DirectoryChanged(DirectoryChangedEventArgs args)
        {
            _service = args.Service;
            _provider = args.Provider;
            _directory = await _service.GetDirectoryAsync(_provider, args.Directory.Path);

            var count = 0L;
            if (_directory.Directories != null)
                count += _directory.Directories.Count;
            if (_directory.Files != null)
                count += _directory.Files.Count;

            Count = count;
        }
    }
}
