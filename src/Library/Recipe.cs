//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_GRASP_And_SOLID
{
    // La clase Recipe implementa la interfaz IRecipeContent, cumpliendo con el Principio de Inversión de Dependencias (DIP)
    public class Recipe : IRecipeContent
    {
        // Lista de pasos de la receta, utiliza el tipo BaseStep para cumplir con el Principio de Abierto/Cerrado (OCP)
        private IList<BaseStep> steps = new List<BaseStep>();
        public bool enProceso;
        public bool Cocinadito { get;  private set; }

        public Product FinalProduct { get; set; }

        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result =+ step.GetStepCost();
            }

            return result;
        }
        
        public int SaberTiempoCoccion()
        {
            int counter = 0;
            foreach (var step in steps)
            {
                counter += step.Time;
            }
            return counter;
        }
        
        public void CoccionTrue()
        {
            Cocinadito = true;
        }
        
        public async void Coccion()
        {
            int tiempo_de_coccion = SaberTiempoCoccion();
            if (enProceso)
            {
                throw new InvalidOperationException("Ya se encuentra esa receta en produccion");
            }
            await Task.Delay(tiempo_de_coccion);
            enProceso = true;
            TemporizadorDeCoccion tempo = new TemporizadorDeCoccion(this);
            tempo.TimeOut();
        }
    }
}