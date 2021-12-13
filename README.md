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
* Haverá um número fixo de pessoas a serem resgatadas pelo bombeiro. Para chegar em uma pessoa, o bombeiro deve abrir portas clickando nelas
com o LMB, para derrubar as portas trancadas, é necessário repetir o clique 4 vezes, ocorrendo um som a cada batida,
até que a parte seja derrubada. Algumas partes da casa estão bloqueadas por fogo, e você não consegue passar sem apagar
o fogo. Para apagar o fogo, ao mirar nele, aparecerá uma barra em sua tela dizendo para segurar o LMB até que a barra
se encha, após isto, o fogo se apagará e você poderá passar. Quando encontrar alguém que precise ser salvo, a barra
pedindo que você segure o botão do mouse aparecerá novamente, e fazendo isto, você agora está carregando a pessoa. 
Seu oxigênio desse mais rápido e você se move mais lentamente, leve-o rapidamente para a safe-zone!

## Checkpoints

# 22/11/21
* Para fazer com que os fogos apaguem, diferentemente do jogo original que era singleplayer, esta mecânica não era presente. 
Para bloquear o caminho do player, havia um gameojbect vazio em uma área com um BoxCollider adicionado para haver colisão.
O jeito que achamos para que o fogo apagasse corretamente e deixasse de bloquear o caminho, foi tirando estas HotZones,
colocando um BoxCollider no gameobject do Fogo em si. Desta forma, o interagível é Fogo em si e não algo separado, fazendo
com que seja muito mais fácil apagar o Fogo. Como fizemos foi parecido com o método com o qual salvamos os NPCs presentes.
Quando o player segura o botão do mouse, ele enche a barra na UI e juntamente uma variável, que quando chega a 100, checa 
o hit.transform do objeto mirado, e da Destroy no GameObject. Alguns problemas na UI estão presentes, como quando a barra
termina de preencher e o fogo é apagado, o fogo some, mas a barra na UI continua presente, e não desaparece, muitas vezes
a variável também continua em 100 e por isso, quando se ia salvar o NPC, acontecia que o player carregava o NPC instantâneamente
sem a necessidade de encher a barra novamente.

# 29/11/21
* Spawns randômicos implementandos corretamente e funcionando, o método foi deixar os npcs desativados e 
ativa-los randomicamente durante o início do jogo. Fogo está apagando corretamente, mas o problema com a barra e a variável
que enchem ainda estão presentes. Estamos com um problema na câmera agora, onde a sensibilidade do eixo X é normal, mas a do
eixo Y é consideravelmente mais baixa, chegando até a atrapalhar bastante. Começamos a ajustar as portas que anteriormente 
não eram acessíveis, para que elas sejam derrubáveis, mas ela estão com problema, e não estão caindo como deveriam.

# 6/12/21
* Mecânica do fogo ajustada. Alguns problemas presentes na UI ainda, principalmente na barra que enche quando o botão
do mouse é segurado.  Portas finalmente arrumadas para quebrarem adequadamente, ajuste no funcionamento dos NPCs devido à erros presentes.
Spawn dos NPCs depois de bastante pensamento, foi revertido à modo fixo, pois mesmo com o script rodando, devido ao modo
como o Network Idendity funciona, spawnando os NPCs independente de como o script funciona.

