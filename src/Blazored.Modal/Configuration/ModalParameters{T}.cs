using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Blazored.Modal
{
    public class ModalParameters<TComponent> : ModalParameters, IEnumerable<KeyValuePair<Expression<Func<TComponent, object>>, object>>
    {
        public ModalParameters<TComponent> Add<TParameter>(Expression<Func<TComponent, TParameter>> parameterExpression, TParameter value)
        {
            if (parameterExpression.Body is MemberExpression memberExpression)
            {
                var property = typeof(TComponent).GetProperty(memberExpression.Member.Name);
                if (!property.CustomAttributes.Any(a => a.AttributeType == typeof(ParameterAttribute)))
                {
                    throw new InvalidOperationException($"Property {memberExpression.Member.Name} is not a component parameter.");
                }
                Add(memberExpression.Member.Name, value);
            }
            else
            {
                throw new InvalidOperationException("Expression is not a member");
            }

            return this;
        }

        /// <inheritdoc />
        public new IEnumerator<KeyValuePair<Expression<Func<TComponent, object>>, object>> GetEnumerator()
        {
            var parameter = Expression.Parameter(typeof(TComponent));

            return ((Dictionary<string, object>)this).ToDictionary(
                kvp => (Expression<Func<TComponent, object>>)Expression.Lambda(Expression.Convert(Expression.Property(parameter, kvp.Key), typeof(object)), parameter),
                kvp => kvp.Value)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
