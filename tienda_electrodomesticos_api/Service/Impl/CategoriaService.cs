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

        public async Task<Categoria> GuardarCategoria(Categoria categoria, IFormFile? imagen = null)
        {
            if (imagen != null && imagen.Length > 0)
            {
                // Generar un nombre único para evitar duplicados
                var fileName = Guid.NewGuid() + Path.GetExtension(imagen.FileName);

                // Guardar el nombre en la entidad (se guarda en la DB)
                categoria.ImagenNombre = fileName;

                // Guardar archivo en la carpeta
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/category_img", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await imagen.CopyToAsync(stream);
            }

            await _categoriaRepository.Agregar(categoria);
            return categoria;
        }


        public async Task ActualizarCategoria(Categoria categoria, IFormFile? imagen = null)
        {
            // Obtener la categoría actual de la BD
            var catDb = await _categoriaRepository.ObtenerPorId(categoria.Id);
            if (catDb == null) throw new Exception("Categoría no encontrada");

            // Actualizar datos
            catDb.Nombre = categoria.Nombre;
            catDb.IsActive = categoria.IsActive;

            // Actualizar imagen solo si hay nueva
            if (imagen != null && imagen.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imagen.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/category_img", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await imagen.CopyToAsync(stream);

                catDb.ImagenNombre = fileName;
            }

            await _categoriaRepository.Actualizar(catDb);
        }




        public async Task<bool> ExisteCategoria(string nombre)
        {
            return await _categoriaRepository.ExistePorNombre(nombre);
        }

        public async Task<List<Categoria>> GetAllCategorias()
        {
            return await _categoriaRepository.ListarTodas();
        }

        public async Task<List<Categoria>> GetAllCategoriasTodas()
        {
            return await _categoriaRepository.ListarTodasCategorias();
        }


        public async Task<bool> EliminarCategoria(int id)
        {
            var categoria = await _categoriaRepository.ObtenerPorId(id);
            if (categoria != null)
            {
                // Borrar la imagen física si existe
                if (!string.IsNullOrEmpty(categoria.ImagenNombre))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/category_img", categoria.ImagenNombre);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                // Borrar el registro de la base de datos
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
