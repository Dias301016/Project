//Реализация паттерна Unit of Work.
//Класс предназначен для упрощения работы с различными репозиториями (FuelRepository, TankRepository, OperationRepository). 
//Это позволяет гарантировать, что все репозитории будут использовать один и тот же контекст данных. 
//Доступ к конкретному репозитарию в контроллерах осуществляется как вызов свойства экземпляра этого класса.
using System;

namespace ToplivoCodeFirst.Models
{
    public class UnitOfWork: IDisposable
    {
        private ToplivoContext db = new ToplivoContext();
        private FuelRepository fuelRepository;
        private TankRepository tankRepository;
        private OperationRepository operationRepository;

        public FuelRepository Fuels
        {
            get
            {
                if (fuelRepository == null)
                    fuelRepository = new FuelRepository(db);
                return fuelRepository;
            }
        }

        public TankRepository Tanks
        {
            get
            {
                if (tankRepository == null)
                    tankRepository = new TankRepository(db);
                return tankRepository;
            }
        }

        public OperationRepository Operations
        {
            get
            {
                if (operationRepository == null)
                    operationRepository = new OperationRepository(db);
                return operationRepository;
            }
        }


        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}