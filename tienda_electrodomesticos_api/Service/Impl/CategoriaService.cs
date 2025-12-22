namespace tienda_electrodomesticos_api.Service.Impl
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using tienda_electrodomesticos_api.Models;
    using tienda_electrodomesticos_api.Repositorio.Interfaces;
    using tienda_electrodomesticos_api.Service.Interfaces;

    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Categoria> GuardarCategoria(Categoria categoria)
        {
            await _categoriaRepository.Agregar(categoria);
            return categoria;
        }

        public async Task ActualizarCategoria(Categoria categoria)
        {
            await _categoriaRepository.Actualizar(categoria);
        }


        public async Task<bool> ExisteCategoria(string nombre)
        {
            return await _categoriaRepository.ExistePorNombre(nombre);
        }

        public async Task<List<Categoria>> GetAllCategorias()
        {
            return await _categoriaRepository.ListarTodas();
        }

        public async Task<bool> EliminarCategoria(int id)
        {
            var categoria = await _categoriaRepository.ObtenerPorId(id);
            if (categoria != null)
            {
                await _categoriaRepository.Eliminar(categoria);
                return true;
            }
            return false;
        }

        public async Task<Categoria?> ObtenerCategoriaPorId(int id)
        {
            return await _categoriaRepository.ObtenerPorId(id);
        }

        public async Task<List<Categoria>> GetAllActiveCategorias()
        {
            return await _categoriaRepository.ListarActivas();
        }
    }

}
