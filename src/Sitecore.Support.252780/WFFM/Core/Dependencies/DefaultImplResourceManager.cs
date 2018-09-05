namespace Sitecore.Support.WFFM.Core.Dependencies
{
  using Sitecore.Diagnostics;
  using Sitecore.WFFM.Abstractions.Shared;
  using System;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Resources;

  public class DefaultImplResourceManager : IResourceManager
  {
    private readonly ResourceManager rm;
    private readonly ITranslationProvider translationProvider;

    public DefaultImplResourceManager(string resourceName, ITranslationProvider translationProvider)
    {
      Assert.IsNotNull(translationProvider, "Dependency translationProvider is null");
      this.rm = new ResourceManager(resourceName, Assembly.GetExecutingAssembly());
      Assert.IsNotNull(this.rm, "Resource manager for " + resourceName + " is not initialized");
      this.translationProvider = translationProvider;
    }

    public UnmanagedMemoryStream GetObject(string resIdentifier)
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      string name = executingAssembly.GetManifestResourceNames().SingleOrDefault<string>(resource => resource.EndsWith(resIdentifier, StringComparison.InvariantCultureIgnoreCase));
      if (name != null)
      {
        return (UnmanagedMemoryStream)executingAssembly.GetManifestResourceStream(name);
      }
      return null;
    }

    public string GetString(string resIdentifier)
    {
      return this.rm.GetString(resIdentifier);
    }

    public string Localize(string resIdentifier)
    {
      return this.translationProvider.Text(this.GetString(resIdentifier) ?? string.Empty);
    }

    public string Localize(string resIdentifier, params string[] parameters)
    {
      return this.translationProvider.Text(this.GetString(resIdentifier), parameters);
    }
  }
}