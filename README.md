# pets-care
Repositório do projeto final da aceleração de C# da Trybe, feita em grupo

## Utilização do projeto

Para utilizar o projeto, basta clonar o repositório e executar o seguinte comando dentro da raiz do projeto: `dotnet restore`, e na pasta `src/pets-care` executar o comando `dotnet run`.

#### Retorna todos os clientes
```
  GET /client/
```

#### Retorna um cliente
```
  GET /client/${id}
```
| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | **Obrigatório**. O ID do cliente que você quer ver |

#### Retorna o QR Code de um cliente
```
  GET /client/qrcode/${id}
```
| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | **Obrigatório**. O ID do cliente que você quer ver |

#### Retorna a string do QR Code de um cliente
```
  GET /client/qrcode/${id}/string
```
| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | **Obrigatório**. O ID do cliente que você quer ver |

#### Adiciona um cliente
```
  POST /client/
```
Passando no body um json no formato:
```json
    {
        "name": "string",
        "email": "user@example.com",
        "cep": "04321020",
        "adress": "string",
        "password": "string",
        "confirmpassword": "string"
    } 
```

#### Modifica as informações do cliente
```
  PUT /client/${id}
```
| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | **Obrigatório**. O ID do cliente que você quer alterar |

Passando no body um json no formato:
```json
    {
        "name": "otherstring",
        "email": "other@email.com"
    }
```

#### Deleta um cliente
```
  DELETE /client/${id}
```
| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | **Obrigatório**. O ID do cliente que você quer deletar |

#
### Rotas referentes aos pets

#### Retorna todos os pets
```
  GET /pet/
```

#### Retorna um pet
```
  GET /pet/${id}
```
| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | **Obrigatório**. O ID do pet que você quer ver |

#### Retorna os pets de um cliente
```
  GET pet/client/{id}/pets
```
| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | **Obrigatório**. O ID do cliente cujos pets você quer ver |


#### Adiciona um pet
```
  POST /pet/
```
Passando no body um json no formato:
```json
    {
        "name": "Napoleao",
        "size": "Medium",
        "breed": "Pinscher",
        "birthdate": "2023-003-001"
    }
```

#### Deleta um pet
```
  DELETE /pet/${id}
```
| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | **Obrigatório**. O ID do pet que você quer deletar |
