﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{

    public enum Player { A, B }

    [Serializable]
    public class Piece
    {
        public enum Type { SWORD, SPEAR, HORSE }
        public Player owner;
        public int maxHealth;
        public int health;
        public int attack;
        public int def;
        public Type type;

        public string uid; // unique id for a piece

        public Piece(
            string uid,
            Player owner,
            Type type = Type.SWORD
        )
        {
            this.uid = uid;
            this.owner = owner;
            this.type = type;
            switch (type)
            {
                case Type.SWORD:
                    this.health = 100; this.attack = 5; this.def = 5;
                    break;
                case Type.SPEAR:
                    this.health = 100; this.attack = 7; this.def = 3;
                    break;
                case Type.HORSE:
                    this.health = 80; this.attack = 9; this.def = 1;
                    break;
                default:
                    this.health = 100; this.attack = 5; this.def = 5;
                    break;
            }
            this.maxHealth = this.health;
        }

        public override int GetHashCode()
        {
            return uid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this == (Piece)obj;
        }

        public static bool operator ==(Piece a, Piece b)
        {
            return a.uid == b.uid;
        }

        public static bool operator !=(Piece a, Piece b)
        {
            return !(a == b);
        }

        public bool IsCounteredBy(Piece a)
        {
            if (type == Type.HORSE && a.type == Type.SWORD) return true;
            else if (type == Type.HORSE && a.type == Type.SPEAR) return true;
            else if (type == Type.SWORD && a.type == Type.HORSE) return true;
            else return false;
        }
    }

    [Serializable]
    public class Move
    {
        public enum Direction { UP, DOWN, LEFT, RIGHT, NONE }
        public Piece piece;
        public Direction direction;

        public Move(Piece piece, Direction direction = Move.Direction.NONE)
        {
            this.piece = piece;
            this.direction = direction;
        }

        public static string DirectionToString(Direction direction)
        {
            switch (direction)
            {
                case Direction.UP: return "UP";
                case Direction.DOWN: return "DOWN";
                case Direction.LEFT: return "LEFT";
                case Direction.RIGHT: return "RIGHT";
                case Direction.NONE: return "NONE";
                default: return "NONE";
            }
        }

        public static Direction OppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.UP: return Direction.DOWN;
                case Direction.DOWN: return Direction.UP;
                case Direction.LEFT: return Direction.RIGHT;
                case Direction.RIGHT: return Direction.LEFT;
                case Direction.NONE: return Direction.NONE;
                default: return Direction.NONE;
            }
        }
    }

    [Serializable]
    public class Square
    {
        public int row;
        public int col;
        public Square(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        public override int GetHashCode()
        {
            return row * 31 + col;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this == (Square)obj;
        }

        public static bool operator ==(Square a, Square b)
        {
            return a.row == b.row && a.col == b.col;
        }

        public static bool operator !=(Square a, Square b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return string.Format("[Square row:{0} col:{1}]", row, col);
        }
    }

    public class Board
    {
        int numRows;
        int numCols;
        Dictionary<Piece, Square> pieceToSquare = new Dictionary<Piece, Square>();

        public Board(int rows, int cols)
        {
            numRows = rows;
            numCols = cols;
        }

        public Square NextSquare(Square currentSquare, Move.Direction direction)
        {
            int row, col;
            switch (direction)
            {
                case Move.Direction.UP:
                    row = currentSquare.row > 0 ? currentSquare.row - 1 : 0;
                    return new Square(row, currentSquare.col);
                case Move.Direction.DOWN:
                    row = currentSquare.row < numRows - 1 ? currentSquare.row + 1 : numRows - 1;
                    return new Square(row, currentSquare.col);
                case Move.Direction.LEFT:
                    col = currentSquare.col > 0 ? currentSquare.col - 1 : 0;
                    return new Square(currentSquare.row, col);
                case Move.Direction.RIGHT:
                    col = currentSquare.col < numCols - 1 ? currentSquare.col + 1 : numCols - 1;
                    return new Square(currentSquare.row, col);
            }
            return currentSquare;
        }

        public void SetPieceAtSquare(Piece piece, Square square)
        {
            pieceToSquare.Remove(piece);
            pieceToSquare[piece] = square;
        }

        public void RemovePieceFromBoard(Piece piece)
        {
            pieceToSquare.Remove(piece);
        }

        public Square NextSquare(Move move)
        {
            Square currentSquare = GetCurrentSquare(move.piece);
            Move.Direction direction = move.direction;
            return NextSquare(currentSquare, direction);
        }

        public bool HasPiece(Piece piece)
        {
            return pieceToSquare.ContainsKey(piece);
        }

        public Square GetCurrentSquare(Piece piece)
        {
            return pieceToSquare[piece];
        }
    }

}
