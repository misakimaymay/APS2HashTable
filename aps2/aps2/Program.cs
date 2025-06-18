using System;
using System.Collections.Generic;

public class Paciente
{
    public string CPF { get; set; }
    public string Nome { get; set; }
    public double PressaoArterial { get; set; }
    public double Temperatura { get; set; }
    public double Oxigenacao { get; set; }
    public string Prioridade { get; set; }

    public Paciente(string cpf, string nome, double pa, double temp, double ox)
    {
        CPF = cpf;
        Nome = nome;
        PressaoArterial = pa;
        Temperatura = temp;
        Oxigenacao = ox;
        DefinirPrioridade();
    }

    public void AtualizarDados(double pa, double temp, double ox)
    {
        PressaoArterial = pa;
        Temperatura = temp;
        Oxigenacao = ox;
        DefinirPrioridade();
    }

    public void DefinirPrioridade()
    {
        if (PressaoArterial > 18 || Temperatura > 39 || Oxigenacao < 90)
            Prioridade = "Vermelha";
        else if (PressaoArterial < 12 || PressaoArterial > 14 || Temperatura < 36 || Temperatura > 37.5 || Oxigenacao < 95)
            Prioridade = "Amarela";
        else
            Prioridade = "Verde";
    }

    public void Exibir()
    {
        switch (Prioridade)
        {
            case "Vermelha":
                Console.ForegroundColor = ConsoleColor.Red; break;
            case "Amarela":
                Console.ForegroundColor = ConsoleColor.Yellow; break;
            case "Verde":
                Console.ForegroundColor = ConsoleColor.Green; break;
        }
        Console.WriteLine($"CPF: {CPF} | Nome: {Nome} | PA: {PressaoArterial} | Temp: {Temperatura} | O₂: {Oxigenacao} | Prioridade: {Prioridade}");
        Console.ResetColor();
    }
}

public class TabelaHash
{
    private int Capacidade;
    private LinkedList<KeyValuePair<string, Paciente>>[] Buckets;

    public TabelaHash(int capacidade)
    {
        Capacidade = capacidade;
        Buckets = new LinkedList<KeyValuePair<string, Paciente>>[Capacidade];
        for (int i = 0; i < Capacidade; i++)
            Buckets[i] = new LinkedList<KeyValuePair<string, Paciente>>();
    }

    private int Hash(string key)
    {
        return (Math.Abs(key.GetHashCode()) % Capacidade);
    }

    public void Inserir(Paciente paciente)
    {
        int idx = Hash(paciente.CPF);
        foreach (var kv in Buckets[idx])
        {
            if (kv.Key == paciente.CPF)
            {
                Console.WriteLine("CPF já cadastrado!");
                return;
            }
        }
        Buckets[idx].AddLast(new KeyValuePair<string, Paciente>(paciente.CPF, paciente));
        Console.WriteLine("Paciente inserido com sucesso!");
    }

    public Paciente Buscar(string cpf)
    {
        int idx = Hash(cpf);
        foreach (var kv in Buckets[idx])
        {
            if (kv.Key == cpf)
                return kv.Value;
        }
        return null;
    }

    public bool Atualizar(string cpf, double pa, double temp, double ox)
    {
        Paciente p = Buscar(cpf);
        if (p != null)
        {
            p.AtualizarDados(pa, temp, ox);
            return true;
        }
        return false;
    }

    public bool Remover(string cpf)
    {
        int idx = Hash(cpf);
        var node = Buckets[idx].First;
        while (node != null)
        {
            if (node.Value.Key == cpf)
            {
                Buckets[idx].Remove(node);
                return true;
            }
            node = node.Next;
        }
        return false;
    }

    public void ExibirTabela()
    {
        for (int i = 0; i < Capacidade; i++)
        {
            Console.WriteLine($"Bucket [{i}]:");
            foreach (var kv in Buckets[i])
            {
                kv.Value.Exibir();
            }
            if (Buckets[i].Count == 0)
                Console.WriteLine("  (vazio)");
        }
    }

    public List<Paciente> TodosPacientes()
    {
        List<Paciente> lista = new List<Paciente>();
        for (int i = 0; i < Capacidade; i++)
            foreach (var kv in Buckets[i])
                lista.Add(kv.Value);
        return lista;
    }
}

public class SistemaClinico
{
    private TabelaHash tabela;
    private Queue<string> filaTriagem;
    private Stack<string> historicoAtendimentos;

    public SistemaClinico(int capacidade)
    {
        tabela = new TabelaHash(capacidade);
        filaTriagem = new Queue<string>();
        historicoAtendimentos = new Stack<string>();
    }

    public void Menu()
    {
        while (true)
        {
            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine("1. Cadastrar paciente");
            Console.WriteLine("2. Buscar paciente por CPF");
            Console.WriteLine("3. Atualizar dados clínicos");
            Console.WriteLine("4. Remover paciente");
            Console.WriteLine("5. Exibir tabela hash");
            Console.WriteLine("6. Exibir fila de triagem");
            Console.WriteLine("7. Atender próximo paciente (simular atendimento)");
            Console.WriteLine("8. Exibir histórico de atendimentos");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha: ");
            string opt = Console.ReadLine();

            switch (opt)
            {
                case "1":
                    CadastrarPaciente();
                    break;
                case "2":
                    BuscarPaciente();
                    break;
                case "3":
                    AtualizarPaciente();
                    break;
                case "4":
                    RemoverPaciente();
                    break;
                case "5":
                    tabela.ExibirTabela();
                    break;
                case "6":
                    ExibirFila();
                    break;
                case "7":
                    AtenderPaciente();
                    break;
                case "8":
                    ExibirHistorico();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    private void CadastrarPaciente()
    {
        Console.Write("CPF: "); string cpf = Console.ReadLine();
        Console.Write("Nome: "); string nome = Console.ReadLine();
        Console.Write("Pressão Arterial: "); double pa = double.Parse(Console.ReadLine());
        Console.Write("Temperatura: "); double temp = double.Parse(Console.ReadLine());
        Console.Write("Oxigenação: "); double ox = double.Parse(Console.ReadLine());

        Paciente p = new Paciente(cpf, nome, pa, temp, ox);
        tabela.Inserir(p);
        filaTriagem.Enqueue(cpf);
    }

    private void BuscarPaciente()
    {
        Console.Write("CPF: "); string cpf = Console.ReadLine();
        Paciente p = tabela.Buscar(cpf);
        if (p != null) p.Exibir();
        else Console.WriteLine("Paciente não encontrado.");
    }

    private void AtualizarPaciente()
    {
        Console.Write("CPF: "); string cpf = Console.ReadLine();
        Console.Write("Nova Pressão Arterial: "); double pa = double.Parse(Console.ReadLine());
        Console.Write("Nova Temperatura: "); double temp = double.Parse(Console.ReadLine());
        Console.Write("Nova Oxigenação: "); double ox = double.Parse(Console.ReadLine());
        if (tabela.Atualizar(cpf, pa, temp, ox))
            Console.WriteLine("Atualizado com sucesso!");
        else
            Console.WriteLine("Paciente não encontrado.");
    }

    private void RemoverPaciente()
    {
        Console.Write("CPF: "); string cpf = Console.ReadLine();
        if (tabela.Remover(cpf))
            Console.WriteLine("Removido com sucesso!");
        else
            Console.WriteLine("Paciente não encontrado.");
    }

    private void ExibirFila()
    {
        Console.WriteLine("Fila de Triagem:");
        foreach (var cpf in filaTriagem)
        {
            Paciente p = tabela.Buscar(cpf);
            if (p != null) p.Exibir();
        }
        if (filaTriagem.Count == 0)
            Console.WriteLine("Fila vazia.");
    }

    private void AtenderPaciente()
    {
        if (filaTriagem.Count == 0)
        {
            Console.WriteLine("Nenhum paciente na fila.");
            return;
        }
        string cpf = filaTriagem.Dequeue();
        Paciente p = tabela.Buscar(cpf);
        if (p != null)
        {
            Console.WriteLine("Atendendo paciente:");
            p.Exibir();
            historicoAtendimentos.Push(cpf);
        }
        else
        {
            Console.WriteLine("Paciente não encontrado.");
        }
    }

    private void ExibirHistorico()
    {
        Console.WriteLine("Histórico de Atendimentos:");
        foreach (var cpf in historicoAtendimentos)
        {
            Paciente p = tabela.Buscar(cpf);
            if (p != null) p.Exibir();
        }
        if (historicoAtendimentos.Count == 0)
            Console.WriteLine("Histórico vazio.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        SistemaClinico sistema = new SistemaClinico(10);
        sistema.Menu();
    }
}