﻿using Backend.Data.Context;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories
{
    public class EnrolementRepository : IEnrolementRepository
    {
        protected readonly DataContext _context;

        public EnrolementRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Enrolement> CreateEnrolement(Enrolement enrolement)
        {
            EntityEntry<Enrolement> result = await _context.Enrolements.AddAsync(enrolement);
            _context.SaveChanges();
            if (result != null)
            {
                return result.Entity;
            }
            return null;
        }
    }
}
