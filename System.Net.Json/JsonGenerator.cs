using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using SR = System.Reflection;

using System.Reflection.Emit;
using SRE = System.Reflection.Emit;

using System.CodeDom;
using System.CodeDom.Compiler;

namespace System.Net.Json
{


    [global::System.Serializable]
    public class GeneratorException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        private CompilerResults _results;
        public CompilerResults CompilerResults { get { return _results; } }

        public GeneratorException(string message, CompilerResults results)
            : base(message)
        {
            _results = results;
        }

        public GeneratorException() { }
        public GeneratorException(string message) : base(message) { }
        public GeneratorException(string message, Exception inner) : base(message, inner) { }
        protected GeneratorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    /// <summary>
    /// Generates small library for interoperation with json text.
    /// </summary>
    public class JsonGenerator
    {
        private void GenerateDefaultConstructor(CodeTypeDeclaration classObject, JsonObject jsonObject)
        {
            CodeConstructor ctor = new CodeConstructor();
            classObject.Members.Add(ctor);
            ctor.Attributes = MemberAttributes.Public;


            //RootObject = new JsonObjectCollection();
            CodeAssignStatement assignRootObject = new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "RootObject"),
                new CodeObjectCreateExpression(jsonObject.GetType()));

            ctor.Statements.Add(assignRootObject);


            
            //JsonStringValue name = new JsonStringValue("Name");
            //JsonStringValue surName = new JsonStringValue("SurName");
            //RootObject.Add(name);
            //RootObject.Add(surName);
        }

        private void GenerateConstructorWithTextParameter(CodeTypeDeclaration classObject, JsonObject jsonObject)
        {
            CodeConstructor ctor = new CodeConstructor();
            classObject.Members.Add(ctor);
            ctor.Attributes = MemberAttributes.Public;
            ctor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "text"));
            // declare parser variable
            CodeVariableDeclarationStatement parserCreate = new CodeVariableDeclarationStatement(
                typeof(JsonTextParser), "parser",
                new CodeObjectCreateExpression(new CodeTypeReference(typeof(JsonTextParser))));
            ctor.Statements.Add(parserCreate);
            // invoke Parse method on parser object
            CodeMethodInvokeExpression invokeParse = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("parser"), "Parse"),
                new CodeVariableReferenceExpression("text"));
            // assign result of Parse method to RootObject
            CodeAssignStatement assignObject = new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "RootObject"),
                new CodeCastExpression(new CodeTypeReference(jsonObject.GetType()), invokeParse));

            ctor.Statements.Add(assignObject);
            ctor.Statements.Add(new CodeMethodReturnStatement());
        }

        private void GenerateToStringDefaultMethod(CodeTypeDeclaration classObject)
        {
            CodeMemberMethod tostring = new CodeMemberMethod();
            classObject.Members.Add(tostring);
            tostring.Name = "ToString";
            tostring.Attributes = MemberAttributes.Override | MemberAttributes.Public | MemberAttributes.Overloaded;
            tostring.ReturnType = new CodeTypeReference(typeof(string));

            CodeMethodReturnStatement invokeToString = new CodeMethodReturnStatement(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                            new CodeThisReferenceExpression(), "RootObject"),
                            "ToString"));
            tostring.Statements.Add(invokeToString);
        }

        private void GenerateParseStaticMethod(CodeTypeDeclaration classObject)
        {
            CodeMemberMethod parse = new CodeMemberMethod();
            classObject.Members.Add(parse);
            parse.Name = "Parse";
            parse.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            parse.ReturnType = new CodeTypeReference(classObject.Name);
            parse.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "text"));
            parse.Statements.Add(new CodeMethodReturnStatement(new CodeObjectCreateExpression(new CodeTypeReference("Person"), new CodeArgumentReferenceExpression("text"))));
        }


        public void GenerateLibrary(string objectName, JsonObject jsonObject)
        {
            GenerateLibrary(objectName, jsonObject);
        }

        /// <summary>
        /// Only his method is public
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="jsonObject"></param>
        public void GenerateLibrary(string objectName, JsonObject jsonObject, string path)
        {
            CodeCompileUnit ccu = new CodeCompileUnit();
            CodeNamespace ns = new CodeNamespace("System.Net.Json.Generated");
            ns.Imports.Add(new CodeNamespaceImport("System.Net.Json"));
            ccu.ReferencedAssemblies.Add(@"System.Net.Json.dll");
            ccu.Namespaces.Add(ns);

            // root class
            CodeTypeDeclaration rootClass = new CodeTypeDeclaration(objectName);
            ns.Types.Add(rootClass);
            rootClass.TypeAttributes = TypeAttributes.Class | TypeAttributes.Public;


            // root object
            CodeMemberField rootObject = new CodeMemberField(jsonObject.GetType(), "RootObject");
            rootObject.Attributes = MemberAttributes.Family;
            rootClass.Members.Add(rootObject);

            // [rootClass].ToString() method
            GenerateToStringDefaultMethod(rootClass);

            // .ctor()
            GenerateDefaultConstructor(rootClass, jsonObject);

            // .ctor(string text)
            GenerateConstructorWithTextParameter(rootClass, jsonObject);


            // static Parse(string text)
            GenerateParseStaticMethod(rootClass);


            

            // generate nested data.
            if (typeof(JsonObjectCollection) == jsonObject.GetType())
            {
                GenerateObjectCollection(rootClass, (JsonObjectCollection)jsonObject);
            }
            else
            {
                throw new NotImplementedException("Only objects supported in root level, not arrays or other variables.");
            }



            // prepare for compile
            CodeDomProvider cdp = CodeDomProvider.CreateProvider("cs");
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            if (!string.IsNullOrEmpty(path))
            {
                cp.OutputAssembly = System.IO.Path.ChangeExtension(System.IO.Path.Combine(path, objectName), ".dll");
            }
            else
            {
                cp.OutputAssembly = System.IO.Path.ChangeExtension(objectName, ".dll");
            }
            cp.IncludeDebugInformation = false;



            // compile code
            CompilerResults cr = cdp.CompileAssemblyFromDom(cp, ccu);

            if (cr.NativeCompilerReturnValue != 0)
            {
                throw new GeneratorException("Cannot compile your library.\r\n" +
                    "Please send json text from which you trying to generate library to lazureykis@gmail.com",
                    cr);
            }

            // show errors
            //Console.WriteLine("Returned code: " + cr.NativeCompilerReturnValue);
            //Console.WriteLine("Path: " + cr.PathToAssembly);
            //foreach (CompilerError e in cr.Errors)
            //{
                //Console.WriteLine(e);
            //}
        }

        private void GenerateObjectCollection(CodeTypeDeclaration rootClass,
            JsonObjectCollection jsonObject)
        {
            // find default constructor
            CodeConstructor ctor = null;
            foreach (CodeTypeMember m in rootClass.Members)
            {
                CodeConstructor c = m as CodeConstructor;

                if (c != null)
                {
                    if (c.Parameters.Count == 0)
                    {
                        ctor = c;
                        break;
                    }
                }
            }

            if (ctor == null)
            {
                throw new Exception("Cannot find default constructor");
            }

            // enumerate nested data fields
            foreach (JsonObject obj in jsonObject)
            {

                // generate property
                CodeMemberProperty prop = new CodeMemberProperty();
                prop.Name = obj.Name;
                prop.Attributes = MemberAttributes.Public;
                PropertyInfo valueInfo = obj.GetType().GetProperty("Value");
                if (valueInfo == null)
                {
                    throw new GeneratorException("Cannot generate nested arrays or objects. Wait for release :)");
                }

                prop.Type = new CodeTypeReference(valueInfo.PropertyType);
                CodeExpression rootItem = new CodeArrayIndexerExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "RootObject"), new CodePrimitiveExpression(obj.Name));
                CodeExpression castedRootItem = new CodeCastExpression(obj.GetType(), rootItem);
                CodeExpression rootItemValue = new CodeFieldReferenceExpression(castedRootItem, "Value");
                prop.GetStatements.Add(new CodeMethodReturnStatement(rootItemValue));
                prop.SetStatements.Add(new CodeAssignStatement(rootItemValue, new CodePropertySetValueReferenceExpression()));
                rootClass.Members.Add(prop);


                // generate constructor's part
                CodeVariableDeclarationStatement createVar = new CodeVariableDeclarationStatement(obj.GetType(), obj.Name.ToLower(),
                    new CodeObjectCreateExpression(obj.GetType(), new CodePrimitiveExpression(obj.Name)));
                //CodeAssignStatement assignVar = new CodeAssignStatement(codevariable
                ctor.Statements.Add(createVar);

                CodeMethodInvokeExpression invokeAdd = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "RootObject"), "Add"),
                    new CodeVariableReferenceExpression(obj.Name.ToLower()));
                ctor.Statements.Add(invokeAdd);
                //JsonStringValue name = new JsonStringValue("Name");
                //RootObject.Add(name);
            }
        }
    }
}