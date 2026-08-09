using System;
using Microsoft.CodeAnalysis;

namespace H5.Translator.Roslyn;

/// <summary>
/// Thrown when the translator encounters a construct it cannot emit, including
/// language features that do not make sense in a browser environment.
/// </summary>
public sealed class TranslationException : Exception
{
    public TranslationException(string message, Location? location = null) : base(message)
    {
        Location = location;
    }

    public Location? Location { get; }
}

/// <summary>
/// Diagnostic descriptors emitted by the Roslyn translator (prefix H5R).
/// </summary>
internal static class Diagnostics
{
    public static readonly DiagnosticDescriptor Unsupported = new(
        id: "H5R0001",
        title: "Unsupported feature",
        messageFormat: "{0}",
        category: "H5.Translator.Roslyn",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor NotImplemented = new(
        id: "H5R0002",
        title: "Not implemented",
        messageFormat: "Translation of this construct is not implemented yet: {0}",
        category: "H5.Translator.Roslyn",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static Diagnostic Create(DiagnosticDescriptor descriptor, Location? location, params object[] args) =>
        Diagnostic.Create(descriptor, location ?? Location.None, args);
}
