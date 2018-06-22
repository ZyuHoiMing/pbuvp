﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicMvvm.Helpers;
using BasicMvvm.Models;
using BasicMvvm.Services;

namespace BasicMvvm.ViewModels {
    /// <summary>
    /// 主页ViewModel类。
    /// </summary>
    public class MainPageViewModel : INotifyPropertyChanged {

        /// <summary>
        /// 联系人服务。
        /// </summary>
        private IContactService _contactService;

        /// <summary>
        /// 联系人集合。
        /// </summary>
        public ObservableCollection<Contact> ContactCollection {
            get;
            private set;
        }

        /// <summary>
        /// 选中的联系人。
        /// </summary>
        private Contact _selectedContact;

        /// <summary>
        /// 选中的联系人。
        /// </summary>
        public Contact SelectedContact {
            get => _selectedContact;

            set {
                if (_selectedContact == value) {
                    return;
                }

                _selectedContact = value;
                RaisePropertyChanged(nameof(SelectedContact));
            }
        }

        /// <summary>
        /// 刷新命令。
        /// </summary>
        private RelayCommand _listCommand;

        /// <summary>
        /// 刷新命令。
        /// </summary>
        public RelayCommand ListCommand =>
            _listCommand ?? (_listCommand =
                new RelayCommand(async () => { await List(); }));

        /// <summary>
        /// 执行刷新操作。
        /// </summary>
        private async Task List() {
            ContactCollection.Clear();

            var contacts = await _contactService.ListAsync();
            foreach (var contact in contacts) {
                ContactCollection.Add(contact);
            }
        }

        /// <summary>
        /// 保存命令。
        /// </summary>
        private RelayCommand<Contact> _saveCommand;

        /// <summary>
        /// 保存命令。
        /// </summary>
        public RelayCommand<Contact> SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand<Contact>(
                async contact => {
                    var service = _contactService;
                    await service.UpdateAsync(contact);
                }));

        /// <summary>
        /// 详细信息命令。
        /// </summary>
        private RelayCommand<Contact> _showDetailsCommand;

        /// <summary>
        /// 详细信息命令。
        /// </summary>
        public RelayCommand<Contact> ShowDetailsCommand =>
            _showDetailsCommand ?? (_showDetailsCommand =
                new RelayCommand<Contact>(contact => {
                    SelectedContact = contact;
                }));

        public MainPageViewModel(IContactService contactService, INavigationService navigationService) {
            _contactService = contactService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
    }
}