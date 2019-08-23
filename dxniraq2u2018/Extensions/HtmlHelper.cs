using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ControllerContext = Microsoft.AspNetCore.Mvc.ControllerContext;
using ViewEngineResult = Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult;

namespace dxniraq2u2018.Extensions
{
    public static class HtmlHelper
    {
        public static string ToHtml(Controller controller, string viewToRender, object model = null, bool isPartial = false)
        {
            ViewEngineResult result = null;

            result = GetViewResult(controller.ControllerContext, viewToRender, isPartial);
            if (model == null)
                throw new FileNotFoundException("model cannot be null.");

            StringWriter output;
            using (output = new StringWriter())
            {
                var viewData = controller.ViewData;
                if (model != null)
                    viewData.Model = model;
                var viewContext = new ViewContext(controller.ControllerContext, result.View, viewData, controller.TempData, output,new HtmlHelperOptions());
                result.View.RenderAsync(viewContext);
            }

            return output.ToString();
        }

        private static ViewEngineResult GetViewResult(ControllerContext controllerContext, string viewToRender, bool isPartial)
        {
            var engine = controllerContext.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            var getViewResult = engine.GetView(executingFilePath: null, viewPath: viewToRender, isMainPage: !isPartial);
            if (getViewResult.Success)
                return getViewResult;
            var findViewResult = engine.FindView(controllerContext, viewToRender, isMainPage: !isPartial);
            if (findViewResult.Success)
                return findViewResult;
            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);

            var errorMessage = string.Join(
            Environment.NewLine,
            new[] { $"Unable to find view '{viewToRender}'. The following locations were searched:" }.Concat(searchedLocations));
            throw new Exception(errorMessage);
        }
    }
}
