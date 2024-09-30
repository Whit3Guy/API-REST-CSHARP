using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCrudDotNET.Estudantes
{
    public class Estudante
    {   
        // init => após setado, não pode ser mudao
        public Guid Id {get; init;}

        public string Nome {get; private set;}
        public bool Ativo {get; private set;}

        public Estudante(string nome){
            Nome = nome;
            Id = Guid.NewGuid();
            Ativo = true;

        }
    }
}