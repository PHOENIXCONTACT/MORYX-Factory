// Copyright (c) 2022, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using Moryx.AbstractionLayer.Recipes;
using Moryx.Orders.Advice;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moryx.Orders.Facade
{
    // TODO: AL6 Remove extensions
    /// <summary>
    /// Extensions for the <see cref="IOrderManagement"/> facade
    /// </summary>
    public static class OrderFacadeExtensions
    {
        /// <summary>
        /// Bridge extension for assigning or updating operation related information to the <paramref name="operation"/>.
        /// </summary>
        /// <param name="facade">This order management instance</param>
        /// <param name="operation">The <see cref="Operation.Identifier"/> to assign</param>
        public static void Reload(this IOrderManagement facade, Operation operation)
        {
            if (facade is IOrderManagementExtended extended)
            {
                extended.Reload(operation);
                return;
            }                

            throw new NotSupportedException("Instance of order management does not support assignment");
        }

        /// <summary>
        /// Bridge extension for fetching an <see cref="AdviceContext"/> for the <paramref name="operation"/>.
        /// </summary>
        /// <param name="facade">This order management instance</param>
        /// <param name="operation">The <see cref="Operation"/> from which the advice context is taken.</param>
        public static AdviceContext GetAdviceContext(this IOrderManagement facade, Operation operation)
        {
            if (facade is IOrderManagementExtended extended)
                return extended.GetAdviceContext(operation);

            throw new NotSupportedException("Instance of order management does not support advicing");
        }

        /// <summary>
        /// Bridge extension for advising the <paramref name="operation"/>.
        /// </summary>
        /// <param name="facade">This order management instance</param>
        /// <param name="operation">The <see cref="Operation"/> to advice.</param>
        /// <param name="advice">The <see cref="OperationAdvice"/> to apply on the <paramref name="operation"/>.</param>
        public static Task<AdviceResult> TryAdvice(this IOrderManagement facade, Operation operation, OperationAdvice advice)
        {
            if (facade is IOrderManagementExtended extended)
                return extended.TryAdvice(operation, advice);

            throw new NotSupportedException("Instance of order management does not support advicing");
        }

        /// <summary>
        /// Bridge extension for retrieving an array of <see cref="OperationLogMessage"/>s corresponding to the <paramref name="operation"/>.
        /// </summary>
        /// <param name="facade">This order management instance</param>
        /// <param name="operation">The <see cref="Operation"/> to advice.</param>
        public static IReadOnlyCollection<OperationLogMessage> GetLogs(this IOrderManagement facade, Operation operation)
        {
            if (facade is IOrderManagementExtended extended)
                return extended.GetLogs(operation);

            throw new NotSupportedException("Instance of order management does not support retriving logs");
        }

        /// <summary>
        /// Bridge extension for retrieving a set of recipes this OrderManagement can assign to an Operation corresponding to the 
        /// <paramref name="identity"/>.
        /// </summary>
        public static Task<IReadOnlyList<IProductRecipe>> GetAssignableRecipes(this IOrderManagement facade, ProductIdentity identity)
        {
            if (facade is IOrderManagementExtended extended)
                return extended.GetAssignableRecipes(identity);

            throw new NotSupportedException("Instance of order management does not support retriving assignable recipes.");
        }
    }
}
