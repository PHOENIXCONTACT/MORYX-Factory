// Copyright (c) 2024, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Recipes;
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Products;
using Moryx.ControlSystem.Recipes;

namespace Moryx.ControlSystem.Cells
{
    /// <summary>
    /// Extension methods on the <see cref="Session"/> to get product related information, activity details or just provide shortcuts based on the actual session type
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Extension method to get the <see cref="ProductInstance"/> from the <see cref="Process"/> of the <paramref name="session"/>
        /// </summary>
        /// <typeparam name="TProductInstance">Type of the <see cref="ProductInstance"/> that is expected.</typeparam>
        /// <param name="session">The sesion to get the <see cref="ProductInstance"/> from</param>
        /// <returns>
        /// The <see cref="ProductInstance"/> in the session, if the <paramref name="session"/> belongs to a
        /// <see cref="ProductionProcess"/> and the <see cref="ProductionProcess"/> holds a <typeparamref name="TProductInstance"/>;
        /// Otherwise returns null
        /// </returns>
        public static TProductInstance GetProductInstance<TProductInstance>(this Session session) where TProductInstance : ProductInstance
        {
            if (session.Process is not ProductionProcess process) return null;

            return process.ProductInstance as TProductInstance;
        }

        /// <summary>
        /// Extension method to get the <see cref="Activity"/> from the <paramref name="session"/>
        /// </summary>
        /// <typeparam name="TActivityType">Type of the <see cref="Activity"/> that is expected.</typeparam>
        /// <param name="session">The sesion to get the <see cref="Activity"/> from</param>
        /// <returns>
        /// The <see cref="Activity"/> in the session, if the <paramref name="session"/> currently handles an 
        /// Activity of type <typeparamref name="TActivityType"/>; Otherwise returns null
        /// </returns>
        public static TActivityType GetActivity<TActivityType>(this Session session) where TActivityType : Activity
        {
            if (session is ActivityCompleted completed)
                return completed.CompletedActivity as TActivityType;
            if (session is ActivityStart start)
                return start.Activity as TActivityType;

            return null;
        }

        /// <summary>
        /// Extension method to get the last <see cref="Activity"/> from the <paramref name="session"/>
        /// </summary>
        /// <typeparam name="TActivityType">Type of the <see cref="Activity"/> that is expected.</typeparam>
        /// <param name="session">The sesion to get the <see cref="Activity"/> from</param>
        /// <returns>
        /// The last <see cref="Activity"/> in the session, if the <paramref name="session"/> currently handles an 
        /// Activity of type <typeparamref name="TActivityType"/>; Otherwise returns null
        /// </returns>
        public static TActivityType GetLastActivity<TActivityType>(this Session session) where TActivityType : Activity 
            => session.Process.LastActivity<TActivityType>() as TActivityType;

        /// <summary>
        /// Extension method to get the <see cref="ProductType"/> from the <paramref name="session"/>
        /// </summary>
        /// <typeparam name="TProductType">Type of the <see cref="ProductType"/> that is expected.</typeparam>
        /// <param name="session">The session to get the <see cref="ProductType"/> from</param>
        /// <returns>
        /// The target <see cref="ProductType"/> in the session, if it belongs to a <see cref="ProductionProcess"/> 
        /// or holds an <see cref="ISetupRecipe"/> with a <typeparamref name="TProductType"/>; Otherwise returns null.
        /// </returns>
        public static TProductType GetProductType<TProductType>(this Session session) where TProductType : ProductType
        {
            if (session.Process.Recipe is ISetupRecipe setupRecipe)
                return setupRecipe.TargetRecipe.Target as TProductType;

            if (session.Process.Recipe is IProductRecipe prodcutRecipe) 
                return prodcutRecipe.Target as TProductType;
            
            return default;
        }
    }
}
