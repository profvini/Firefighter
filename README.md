# Projeto Firefighter
Repositório do projeto final da cadeira de Jogos Multiplayer

## Autores do Projeto:
* Leonardo Korzenowski Saraiva, Gabriel de Oliveira Pedroso e Thiago Gregory Araujo

# Concept do Jogo:
* Jogo de simulação de bombeiros onde um bombeiro deve resgatar pessoas presas em uma casa pegando fogo. 
Ele deve vasculhar os cômodos de casa em busca destas pessoas. Seu temop é limitado pela barra de oxigênio
que reduz constantemente. Caso o oxigênio chegue ao fim, game over. Portas podem ser abertas para entrar
em quartos da casa, mas algumas estão trancadas e devem ser derrubadas para ganhar acesso ao local.
As pessoas resgatas devem ser levadas de volta ao início onde há a safe zone.

## Mecânicas
* Haverá um número fixo de pessoas a serem resgatadas pelo bombeiro, mas suas localizações de spawn serão 
randomizadas a cada início de jogo. Para chegar em uma pessoa, o bombeiro deve abrir portas clickando nelas
com o LMB, para derrubar as portas trancadas, é necessário repetir o clique 4 vezes, ocorrendo um som a cada batida,
até que a parte seja derrubada. Algumas partes da casa estão bloqueadas por fogo, e você não consegue passar sem apagar
o fogo. Para apagar o fogo, ao mirar nele, aparecerá uma barra em sua tela dizendo para segurar o LMB até que a barra
se encha, após isto, o fogo se apagará e você poderá passar. Quando encontrar alguém que precise ser salvo, a barra
pedindo que você segure o botão do mouse aparecerá novamente, e fazendo isto, você agora está carregando a pessoa. 
Seu oxigênio desse mais rápido e você se move mais lentamente, leve-o rapidamente para a safe-zone!

# Checkpoints
## 29/11/21
* Spawns randômicos implementandos corretamente e funcionando, o método foi deixar os npcs desativados e 
ativa-los randomicamente durante o início do jogo. Fogo está apagando corretamente, mas temos problemas com a câmera
e com a UI. UI não funcionando corretamente e câmera parcialmente.
