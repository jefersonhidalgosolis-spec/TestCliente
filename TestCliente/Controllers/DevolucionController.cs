using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestCliente.ServiceReference1;

namespace TestCliente.Controllers
{
    public class DevolucionController : Controller
    {
        // 1. Vista principal: Listado de devoluciones (Punto 3.2.1)
        public ActionResult Index()
        {
            try
            {
                using (var client = new Service1Client())
                {
                    var lista = client.ListarDevoluciones().ToList();
                    return View(lista);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar las devoluciones: " + ex.Message;
                return View(new List<Devolucion>());
            }
        }

        // 2. Consulta de detalle por IdDevolucion para el Modal (Punto 3.2.1)
        [HttpGet]
        public JsonResult ObtenerDetalle(int idDevolucion)
        {
            try
            {
                using (var client = new Service1Client())
                {
                    var detalle = client.VerDevolucionDetalle(idDevolucion).ToList();
                    return Json(new { exito = true, data = detalle }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error al obtener detalle: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // 3. Vista del formulario de nueva devolución (Punto 3.2.2)
        public ActionResult Crear()
        {
            return View();
        }

        // 4. Buscador de venta y carga de productos disponibles (Apoyo 3.2.2)
        [HttpGet]
        public JsonResult BuscarProductosVenta(int idVenta)
        {
            if (idVenta <= 0)
            {
                return Json(new { exito = false, mensaje = "Ingrese un ID de venta válido mayor que cero." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (var client = new Service1Client())
                {
                    var productos = client.ListarProductosVenta(idVenta).ToList();
                    if (productos == null || productos.Count == 0)
                    {
                        return Json(new { exito = false, mensaje = "No se encontraron productos para la venta #" + idVenta + " o la venta no existe." }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { exito = true, data = productos }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error al consultar la venta: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}