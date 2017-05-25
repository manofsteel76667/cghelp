using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompetetiveProgramming.GameTheory;

namespace CompetetiveProgrammingTests.GameTheory {
    public class StickMove : IMove<StickGame> {

    private int sticks;

    public StickMove(int sticks) {
        this.setSticks(sticks);
    }

    public StickGame Cancel(StickGame game) {
        game.changePlayer();
        game.setSticksRemaining(game.getSticksRemaining() + getSticks());
        log(game, "cancel ");
        return game;
    }

    public StickGame Execute(StickGame game) {
        game.setSticksRemaining(game.getSticksRemaining() - getSticks());
        log(game, "execute ");
        game.changePlayer();
        return game;
    }

    public int getSticks() {
        return sticks;
    }

    private void log(StickGame game, String action) {
        //System.out.println("player " + game.currentPlayer() + " " + action + "move " + sticks + " : sticks remaining= " + game.getSticksRemaining());
    }

    public void setSticks(int sticks) {
        this.sticks = sticks;
    }

    public override String ToString() {
        return "Move[" + sticks + "]";
    }
}
}
