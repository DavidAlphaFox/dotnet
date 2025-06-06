// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

namespace FSharp.Compiler.EditorServices

open System.Collections.Generic
open FSharp.Compiler.Syntax
open FSharp.Compiler.Text

type public CompletionPath = string list * string option // plid * residue

[<RequireQualifiedAccess>]
type public InheritanceContext =
    | Class
    | Interface
    | Unknown

[<RequireQualifiedAccess>]
type public RecordContext =
    | CopyOnUpdate of range: range * path: CompletionPath
    | Constructor of typeName: string
    | Empty
    | New of path: CompletionPath * isFirstField: bool
    | Declaration of isInIdentifier: bool

[<RequireQualifiedAccess>]
type public PatternContext =
    /// <summary>Completing union case field pattern (e.g. fun (Some v| ) -> ) or fun (Some (v| )) -> ). In theory, this could also be parameterized active pattern usage.</summary>
    /// <param name="fieldIndex">Position in the tuple. <see cref="None">None</see> if there is no tuple, with only one field outside of parentheses - `Some v|`</param>
    /// <param name="isTheOnlyField">True when completing the first field in the tuple and no other field is bound - `Case (a|)` but not `Case (a|, b)`</param>
    /// <param name="caseIdRange">Range of the case identifier</param>
    | PositionalUnionCaseField of fieldIndex: int option * isTheOnlyField: bool * caseIdRange: range

    /// Completing union case field pattern (e.g. fun (Some (Value = v| )) -> )
    | NamedUnionCaseField of fieldName: string * caseIdRange: range

    /// Completing union case field identifier in a pattern (e.g. fun (Case (field1 = a; fie| )) -> )
    | UnionCaseFieldIdentifier of referencedFields: string list * caseIdRange: range

    /// Completing a record field identifier in a pattern (e.g. fun { Field1 = a; Fie| } -> )
    | RecordFieldIdentifier of referencedFields: (string * range) list

    /// Any other position in a pattern that does not need special handling
    | Other

[<RequireQualifiedAccess; NoComparison; Struct>]
type MethodOverrideCompletionContext =
    | Class
    | Interface of mInterfaceName: range
    | ObjExpr of mExpr: range

[<RequireQualifiedAccess>]
type public CompletionContext =
    /// Completion context cannot be determined due to errors
    | Invalid

    /// Completing something after the inherit keyword
    | Inherit of context: InheritanceContext * path: CompletionPath

    /// Completing records field
    | RecordField of context: RecordContext

    | RangeOperator

    /// Completing named parameters\setters in parameter list of attributes\constructor\method calls
    /// end of name ast node * list of properties\parameters that were already set
    | ParameterList of pos * HashSet<string>

    /// Completing an attribute name, outside of the constructor
    | AttributeApplication

    | OpenDeclaration of isOpenType: bool

    /// Completing a type annotation (e.g. foo (x: |))
    /// Completing a type application (e.g. typeof<str| >)
    | Type

    /// Completing union case fields declaration (e.g. 'A of stri|' but not 'B of tex|: string')
    | UnionCaseFieldsDeclaration

    /// Completing a type abbreviation (e.g. type Long = int6|)
    /// or a single case union without a bar (type SomeUnion = Abc|)
    | TypeAbbreviationOrSingleCaseUnion

    /// Completing a pattern in a match clause, member/let binding or lambda
    | Pattern of context: PatternContext

    /// Completing a method override (e.g. override this.ToStr|)
    | MethodOverride of
        ctx: MethodOverrideCompletionContext *
        enclosingTypeNameRange: range *
        spacesBeforeOverrideKeyword: int *
        hasThis: bool *
        isStatic: bool *
        spacesBeforeEnclosingDefinition: int

type public ModuleKind =
    { IsAutoOpen: bool
      HasModuleSuffix: bool }

[<RequireQualifiedAccess>]
type public EntityKind =
    | Attribute
    | Type
    | FunctionOrValue of isActivePattern: bool
    | Module of ModuleKind

/// Kind of lexical scope.
[<RequireQualifiedAccess>]
type public ScopeKind =
    | Namespace
    | TopModule
    | NestedModule
    | OpenDeclaration
    | HashDirective

/// Insert open namespace context.
[<RequireQualifiedAccess>]
type public InsertionContext =
    {
        /// Current scope kind.
        ScopeKind: ScopeKind

        /// Current position (F# compiler line number).
        Pos: pos
    }

/// Where open statements should be added.
[<RequireQualifiedAccess>]
type public OpenStatementInsertionPoint =
    | TopLevel
    | Nearest

/// Short identifier, i.e. an identifier that contains no dots.
type public ShortIdent = string

/// An array of `ShortIdent`.
type public ShortIdents = ShortIdent[]

/// `ShortIdent` with a flag indicating if it's resolved in some scope.
type public MaybeUnresolvedIdent = { Ident: ShortIdent; Resolved: bool }

/// Helper data structure representing a symbol, suitable for implementing unresolved identifiers resolution code fixes.
type public InsertionContextEntity =
    {
        /// Full name, relative to the current scope.
        FullRelativeName: string

        /// Ident parts needed to append to the current ident to make it resolvable in current scope.
        Qualifier: string

        /// Namespace that is needed to open to make the entity resolvable in the current scope.
        Namespace: string option

        /// Full display name (i.e. last ident plus modules with `RequireQualifiedAccess` attribute prefixed).
        FullDisplayName: string

        /// Last part of the entity's full name.
        LastIdent: ShortIdent
    }

/// Operations querying the entire syntax tree
module public ParsedInput =
    /// A pattern that collects all sequential expressions to avoid StackOverflowException
    val internal (|Sequentials|_|): SynExpr -> SynExpr list option

    val TryFindExpressionASTLeftOfDotLeftOfCursor: pos: pos * parsedInput: ParsedInput -> (pos * bool) option

    val GetRangeOfExprLeftOfDot: pos: pos * parsedInput: ParsedInput -> range option

    val TryFindExpressionIslandInPosition: pos: pos * parsedInput: ParsedInput -> string option

    val TryGetCompletionContext: pos: pos * parsedInput: ParsedInput * lineStr: string -> CompletionContext option

    val GetEntityKind: pos: pos * parsedInput: ParsedInput -> EntityKind option

    val GetFullNameOfSmallestModuleOrNamespaceAtPoint: pos: pos * parsedInput: ParsedInput -> string[]

    /// Returns `InsertContext` based on current position and symbol idents.
    val TryFindInsertionContext:
        currentLine: int ->
        parsedInput: ParsedInput ->
        partiallyQualifiedName: MaybeUnresolvedIdent[] ->
        insertionPoint: OpenStatementInsertionPoint ->
            (( (* requiresQualifiedAccessParent: *) ShortIdents option (* autoOpenParent: *) *
            ShortIdents option (*  entityNamespace *) *
            ShortIdents option (* entity: *) *
            ShortIdents)
                -> (InsertionContextEntity * InsertionContext)[])

    /// Returns `InsertContext` based on current position and symbol idents.
    val FindNearestPointToInsertOpenDeclaration:
        currentLine: int ->
        parsedInput: ParsedInput ->
        entity: ShortIdents ->
        insertionPoint: OpenStatementInsertionPoint ->
            InsertionContext

    /// Returns long identifier at position.
    val GetLongIdentAt: parsedInput: ParsedInput -> pos: pos -> LongIdent option

    /// Corrects insertion line number based on kind of scope and text surrounding the insertion point.
    val AdjustInsertionPoint: getLineStr: (int -> string) -> ctx: InsertionContext -> pos

// implementation details used by other code in the compiler
module internal SourceFileImpl =

    val IsSignatureFile: string -> bool

    val GetImplicitConditionalDefinesForEditing: isInteractive: bool -> string list
