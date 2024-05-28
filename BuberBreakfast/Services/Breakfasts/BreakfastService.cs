using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BuberBreakfast.Models;
using BuberBreakfast.Data;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

public class BreakfastService : IBreakfastService
{
    private readonly BreakfastDbContext _context;

    public BreakfastService(BreakfastDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Created>> CreateBreakfast(Breakfast breakfast)
    {
        _context.Breakfasts.Add(breakfast);
        await _context.SaveChangesAsync();
        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteBreakfast(Guid id)
    {
        var breakfast = await _context.Breakfasts.FindAsync(id);
        if (breakfast == null)
        {
            return Errors.Breakfast.NotFound;
        }

        _context.Breakfasts.Remove(breakfast);
        await _context.SaveChangesAsync();
        return Result.Deleted;
    }

    public async Task<ErrorOr<Breakfast>> GetBreakfast(Guid id)
    {
        var breakfast = await _context.Breakfasts.FindAsync(id);
        if (breakfast == null)
        {
            return Errors.Breakfast.NotFound;
        }

        return breakfast;
    }

    public async Task<ErrorOr<UpsertedBreakfast>> UpsertBreakfast(Breakfast breakfast)
    {
        var isNewlyCreated = !_context.Breakfasts.Any(b => b.Id == breakfast.Id);
        _context.Breakfasts.Update(breakfast);
        await _context.SaveChangesAsync();
        return new UpsertedBreakfast(isNewlyCreated);
    }
}
