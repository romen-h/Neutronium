using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeutroniumApi.CodeGen
{
	internal static class MessageHelpers
	{
		private static Diagnostic CreateDebugMessage(string message)
		{
			return Diagnostic.Create(new DiagnosticDescriptor(
				id: "APIGEN0",
				title: "Debug Output",
				messageFormat: message,
				category: "Debug",
				defaultSeverity: DiagnosticSeverity.Warning,
				isEnabledByDefault: true), null);
		}

		private static Diagnostic CreateError(int id, string message)
		{
			return Diagnostic.Create(new DiagnosticDescriptor(
				id: $"APIGEN{id:D4}",
				title: "Error",
				messageFormat: message,
				category: "Error",
				defaultSeverity: DiagnosticSeverity.Error,
				isEnabledByDefault: true), null);
		}

		internal static void Debug(this SourceProductionContext spc, string message)
		{
			spc.ReportDiagnostic(CreateDebugMessage(message));
		}
		
		internal static void Error(this SourceProductionContext spc, int id, string message)
		{
			spc.ReportDiagnostic(CreateError(id, message));
		}
	}
}
