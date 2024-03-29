﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescendig { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnable { get; }

    }
}