﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkspaceServer.Servers.Roslyn.Instrumentation.Contract;

namespace WorkspaceServer.Servers.Roslyn.Instrumentation
{
    public static class ArgumentListGenerator
    {
        public static ArgumentListSyntax GenerateArgumentListForGetProgramState(FilePosition filePosition, params (object, string)[] argumentList)
        {
            var variableInfoArgument = argumentList.Select((a) =>
            {
                var (argument, value) = a;
                return SyntaxFactory.Argument(
                    SyntaxFactory.TupleExpression(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(new[] {
                            ConvertObjectToArgument(argument),
                            SyntaxFactory.Argument(SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(value))) })));
            }).ToList();

            variableInfoArgument.Insert(0, ConvertObjectToArgument(filePosition));

            return SyntaxFactory.ArgumentList(
                SyntaxFactory.SeparatedList<ArgumentSyntax>(variableInfoArgument)
            );
        }

        private static ArgumentSyntax ConvertObjectToArgument(object argument)
        {
            return SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(argument.ToJson())));
        }
    }
}
