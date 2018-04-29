using System;
using System.Collections.Generic;
using MusicStoreDemo.Common.Models.Artist;

namespace MusicStoreDemo.Common.Repositories
{
    public class RepositoryException : Exception{
        public RepositoryException(string message, params string[] errors)
            : base(message){
            this.Errors = errors;
        }
        public string[] Errors { get; set; }
    }

    public class EntityNotFoundRepositoryException : RepositoryException{
        public EntityNotFoundRepositoryException(string message, params string[] errors)
            : base(message, errors){
        }
    }

    public class EntityAlreadyExistsRepositoryException : RepositoryException
    {
        public EntityAlreadyExistsRepositoryException(string message, params string[] errors)
            : base(message, errors)
        {
        }
    }
}