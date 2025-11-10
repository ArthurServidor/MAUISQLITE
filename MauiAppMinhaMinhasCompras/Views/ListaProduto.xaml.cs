using MauiAppMinhaMinhasCompras.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMinhaMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        private ObservableCollection<Produto> lista = new();
        public ListaProduto()
        {
            InitializeComponent();
            lst_produtos.ItemsSource = lista;
        }

        protected async override void OnAppearing()
        {
            try
            {
                lista.Clear();
                App.Db.GetAll().Result.ForEach(prod => lista.Add(prod));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops!", ex.Message, "Ok");
            }
        }

        private void ToolBarItem_Clicked_Add(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new Views.NovoProduto());
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops!", ex.Message, "Ok");
            }
        }

        private async void txt_search_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                string q = e.NewTextValue;
                lst_produtos.IsRefreshing = true;

                lista.Clear();

                List<Produto> tap = await App.Db.Search(q);

                tap.ForEach(prod => lista.Add(prod));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops!", ex.Message, "Ok");
            }
        }

        private void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
        {
            double soma = lista.Sum(prod => prod.Total);
            string msg = $"O total é {soma:C}";

            DisplayAlert("Total dos Produtos", msg, "Ok");
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                MenuItem selecionado = sender as MenuItem;
                Produto produto = selecionado.BindingContext as Produto;

                bool confirm = await DisplayAlert("Tem Certeza?", $"Remover {produto.Descricao}", "Sim",
                    "Não");
                if (confirm)
                {
                    await App.Db.Delete(produto.Id);
                    lista.Remove(produto);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops!", ex.Message, "Ok");
            }
        }
        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Produto p = e.SelectedItem as Produto;

                Navigation.PushAsync(new Views.EditarProduto()
                {
                    BindingContext = p
                });
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops!", ex.Message, "Ok");
            }
        }
        private async void lst_produtos_Refreshing(object? sender, EventArgs e)
        {
            try
            {
                lista.Clear();

                List<Produto> tmp = await App.Db.GetAll();

                tmp.ForEach(prod => lista.Add(prod));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops!", ex.Message, "Ok");
            }
            finally
            {
                lst_produtos.IsRefreshing = false;
            }
        }
    }
}
