using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.BookStore.Books;
using Acme.BookStore.Localization;
using Acme.BookStore.Wpf.Core.Threading;
using Acme.BookStore.Wpf.Services;
using Acme.BookStore.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace Acme.BookStore.Wpf.Tests.Views
{
    public class BookIndex_Tests : BookStoreWpfTestBase
    {
        private BookIndexViewModel _viewModel;
        private ILogger<BookIndexViewModel> _logger;
        private IBooksAppService _bookAppService;
        private IDispatcher _dispatcher;
        private ISnackbarService _snackbarService;
        private IStringLocalizer<BookStoreResource> _localizer;

        public BookIndex_Tests()
        {
            _logger = GetRequiredService<ILogger<BookIndexViewModel>>();
            _bookAppService = GetRequiredService<IBooksAppService>();
            _dispatcher = GetRequiredService<IDispatcher>();
            _snackbarService = GetRequiredService<ISnackbarService>();
            _localizer = GetRequiredService<IStringLocalizer<BookStoreResource>>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            AddTestSubstitution(ref _snackbarService, services);
            AddTestSubstitution(ref _bookAppService, services);
        }

        [UIFact]
        public async Task CanLoadData()
        {
            GivenEmptyViewModel();

            await _viewModel.LoadDataAsync();

            _viewModel.ShouldSatisfyAllConditions(
                vm => vm.ShouldNotBeNull(),
                vm => vm.Books.ShouldNotBeNull(),
                vm => vm.Books.Any().ShouldBeTrue(),
                vm => vm.IsBusy.ShouldBeFalse(),
                vm => vm.IsNotBusy.ShouldBeTrue(),
                vm => vm.GetIsNotBusy().ShouldBe(vm.IsNotBusy),
                vm => vm.Title.ShouldNotBeNullOrEmpty(),
                vm => vm.Subtitle.ShouldBeNullOrEmpty()
            );
        }

        private void GivenEmptyViewModel()
        {
            _viewModel = new BookIndexViewModel(_logger, _bookAppService, _dispatcher, _snackbarService, _localizer);
        }
    }
}
