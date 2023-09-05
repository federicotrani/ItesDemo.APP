﻿using ItesDemo.APP.Models;
using ItesDemo.APP.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ItesDemo.APP.ViewModels;

public class ProductoListaViewModel : BaseViewModel
{
    #region VARIABLES
    private bool isRefreshing;
    private ProductoModel productoSeleccion;

    private ObservableCollection<ProductoModel> productos = new ObservableCollection<ProductoModel>();
    #endregion

    #region CONSTRUCTOR
    public ProductoListaViewModel()
    {
        Title = "Lista Productos";

        Task.Run(async () => { await ConsultarApi(); }).Wait();

        // ConsultarApi();

        RefreshCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            await ConsultarApi();
        });
    }
    #endregion

    #region OBJETOS
    public ObservableCollection<ProductoModel> Productos
    {
        get { return productos; }
        set { SetProperty(ref productos, value); }
    }

    public ProductoModel ProductoSeleccion
    {
        get { return productoSeleccion; }
        set { SetProperty(ref productoSeleccion, value); }
    }

    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set { SetProperty(ref isRefreshing, value); }
    }
    #endregion

    #region METODOS
    private async Task ConsultarApi()
    {
        IsBusy = IsRefreshing = true;

        Productos.Clear();

        var apiClient = new ApiClient();

        var lista = await apiClient.ObtenerProductos();

        /*foreach (AlumnosMateriaModel model in alumnos)
        {
            model.IdEstadoAsistencia = 2;
            model.EstadoAsistencia = "Asistencia Confirmada";
        }*/

        Productos = new ObservableCollection<ProductoModel>(lista);

        IsBusy = IsRefreshing = false;
    }

    private void GoToCancelar()
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
    #endregion

    #region COMANDOS
    public ICommand GoToCancelarCommand => new Command(() => GoToCancelar());
    public ICommand RefreshCommand { get; set; }
    #endregion
}
