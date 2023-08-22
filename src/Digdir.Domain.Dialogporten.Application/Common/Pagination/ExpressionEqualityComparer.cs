﻿using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Digdir.Domain.Dialogporten.Application.Common.Pagination;

/// <summary>
/// This is used internaly to be able to compare expressions for caching.
/// See https://stackoverflow.com/a/19817323 for more information.
/// </summary>
internal sealed class ExpressionEqualityComparer : IEqualityComparer<Expression>, IEqualityComparer<object>
{
    public bool Equals(Expression? x, Expression? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return GetHashCode(x) == GetHashCode(y);
    }

    public new bool Equals(object? x, object? y)
    {
        if (x is not Expression ex || y is not Expression ey)
        {
            return false;
        }

        return Equals(ex, ey);
    }

    public int GetHashCode([DisallowNull] Expression obj) => 
        ExpressionHasher.GetHashCode(obj);

    public int GetHashCode([DisallowNull] object obj) =>
        obj is not Expression ex
            ? obj.GetHashCode()
            : GetHashCode(ex);

    private static class ExpressionHasher
    {
        private const int NullHashCode = 0x61E04917;

        [ThreadStatic]
        private static HashVisitor? _visitor;

        private static HashVisitor Visitor => _visitor ??= new();

        public static int GetHashCode(Expression? e)
        {
            if (e == null)
            {
                return NullHashCode;
            }

            var visitor = Visitor;
            visitor.Reset();
            visitor.Visit(e);
            return visitor.Hash;
        }

        private sealed class HashVisitor : ExpressionVisitor
        {
            internal int Hash { get; private set; }

            internal void Reset()
            {
                Hash = 0;
            }

            private void UpdateHash(int value)
            {
                Hash = (Hash * 397) ^ value;
            }

            private void UpdateHash(object? component)
            {
                if (component == null)
                {
                    UpdateHash(NullHashCode);
                    return;
                }

                if (component is not MemberInfo member)
                {
                    UpdateHash(component.GetHashCode());
                    return;
                }

                var componentHash = member.Name.GetHashCode();
                var declaringType = member.DeclaringType;
                if (declaringType is not null &&
                    declaringType.AssemblyQualifiedName is not null)
                {
                    componentHash = (componentHash * 397) ^ declaringType.AssemblyQualifiedName.GetHashCode();
                }

                UpdateHash(componentHash);
            }

            public override Expression? Visit(Expression? node)
            {
                UpdateHash((int?)node?.NodeType ?? NullHashCode);
                return base.Visit(node);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                UpdateHash(node.Value);
                return base.VisitConstant(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                UpdateHash(node.Member);
                return base.VisitMember(node);
            }

            protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
            {
                UpdateHash(node.Member);
                return base.VisitMemberAssignment(node);
            }

            protected override MemberBinding VisitMemberBinding(MemberBinding node)
            {
                UpdateHash((int)node.BindingType);
                UpdateHash(node.Member);
                return base.VisitMemberBinding(node);
            }

            protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
            {
                UpdateHash((int)node.BindingType);
                UpdateHash(node.Member);
                return base.VisitMemberListBinding(node);
            }

            protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
            {
                UpdateHash((int)node.BindingType);
                UpdateHash(node.Member);
                return base.VisitMemberMemberBinding(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                UpdateHash(node.Method);
                return base.VisitMethodCall(node);
            }

            protected override Expression VisitNew(NewExpression node)
            {
                UpdateHash(node.Constructor);
                return base.VisitNew(node);
            }

            protected override Expression VisitNewArray(NewArrayExpression node)
            {
                UpdateHash(node.Type);
                return base.VisitNewArray(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                UpdateHash(node.Type);
                return base.VisitParameter(node);
            }

            protected override Expression VisitTypeBinary(TypeBinaryExpression node)
            {
                UpdateHash(node.Type);
                return base.VisitTypeBinary(node);
            }
        }
    }
}