using System.Collections.Generic;
using UnityEngine;

public class ShapeMesh
{
    private const float TEXTURE_WIDTH = 1024;
    private const float TEXTURE_HEIGHT = 512;
    
    private const int SHAPE_COUNT = 8;
    
    private readonly Vector2 _lowerLeft = new Vector2(0, 256f);
    private readonly Vector2 _upperRight = new Vector2(256f, 512f);
    
    private readonly List<Quad> _quads = new List<Quad>();

    private Mesh _mesh;


    public ShapeMesh()
    {
        _mesh = new Mesh();
    }

    public Mesh GetMesh()
    {
        return _mesh;
    }

    public void DrawShape(Vector3 position, Vector2 scale, int spriteIndex)
    {
        Vector2 _offset = new Vector2(256f * (spriteIndex % 4), -256f * (spriteIndex / 4));
        Quad shape = new Quad(position, scale, _lowerLeft + _offset, _upperRight + _offset);
        _quads.Add(shape);
    }

    public void UpdateMesh()
    {
        _mesh.vertices = GetMeshVertices();
        _mesh.uv = GetMeshUV();
        _mesh.triangles = GetMeshTriangles();
    }
    
    private void UpdateMeshVertices()
    {
        _mesh.vertices = GetMeshVertices();
    }

    private Vector3[] GetMeshVertices()
    {
        Vector3[] vertices = new Vector3[4 * _quads.Count];
        for (int i = 0; i < _quads.Count; i++)
        {
            vertices[0 + 4 * i] = _quads[i].vertices[0];
            vertices[1 + 4 * i] = _quads[i].vertices[1];
            vertices[2 + 4 * i] = _quads[i].vertices[2];
            vertices[3 + 4 * i] = _quads[i].vertices[3];
        }

        return vertices;
    }

    private Vector2[] GetMeshUV()
    {
        Vector2[] uv = new Vector2[4 * _quads.Count];

        for (int i = 0; i < _quads.Count; i++)
        {
            uv[0 + 4 * i] = _quads[i].uv[0];
            uv[1 + 4 * i] = _quads[i].uv[1];
            uv[2 + 4 * i] = _quads[i].uv[2];
            uv[3 + 4 * i] = _quads[i].uv[3];
        }

        return uv;
    }

    private int[] GetMeshTriangles()
    {
        int[] uv = new int[6 * _quads.Count];

        for (int i = 0; i < _quads.Count; i++)
        {
            uv[0 + 6 * i] = _quads[i].triangles[0] + i * 4;
            uv[1 + 6 * i] = _quads[i].triangles[1] + i * 4;
            uv[2 + 6 * i] = _quads[i].triangles[2] + i * 4;
            uv[3 + 6 * i] = _quads[i].triangles[3] + i * 4;
            uv[4 + 6 * i] = _quads[i].triangles[4] + i * 4;
            uv[5 + 6 * i] = _quads[i].triangles[5] + i * 4;
        }

        return uv;
    }

    private Color[] GetColors()
    {
        Color[] colors = new Color[4 * _quads.Count];
        for (int i = 0; i < _quads.Count; i++)
        {
            colors[0 + 4 * i] = _quads[i].color;
            colors[1 + 4 * i] = _quads[i].color;
            colors[2 + 4 * i] = _quads[i].color;
            colors[3 + 4 * i] = _quads[i].color;
        }

        return colors;
    }
    
    private class Quad
    {
        public Vector3[] vertices { get; } = new Vector3[4];
        public Vector2[] uv => _uv;
        public int[] triangles => _triangles;
        public Color color { get; private set; } = Color.white;
        
        public Vector3 position => _position;
        public Vector2 localScale => _localScale;

        protected readonly Vector2[] _uv = new Vector2[4];
        protected readonly int[] _triangles = new int[6];

        protected Vector3 _position;
        protected Vector2 _localScale;
        protected Vector2 _pixelsLowerLeft;
        protected Vector2 _pixelsUpperRight;

        public Quad(Vector3 position, Vector2 localScale, Vector2 pixelsLowerLeft, Vector2 pixelsUpperRight)
        {
            _position = position;
            _localScale = localScale;
            _pixelsLowerLeft = pixelsLowerLeft;
            _pixelsUpperRight = pixelsUpperRight;

            UpdateVertices();

            _triangles[0] = 0;
            _triangles[1] = 1;
            _triangles[2] = 2;
            _triangles[3] = 3;
            _triangles[4] = 0;
            _triangles[5] = 2;

            _uv[0] = new Vector2(pixelsLowerLeft.x / TEXTURE_WIDTH, pixelsLowerLeft.y / TEXTURE_HEIGHT);
            _uv[1] = new Vector2(pixelsLowerLeft.x / TEXTURE_WIDTH, pixelsUpperRight.y / TEXTURE_HEIGHT);
            _uv[2] = new Vector2(pixelsUpperRight.x / TEXTURE_WIDTH, pixelsUpperRight.y / TEXTURE_HEIGHT);
            _uv[3] = new Vector2(pixelsUpperRight.x / TEXTURE_WIDTH, pixelsLowerLeft.y / TEXTURE_HEIGHT);
        }

        public void SetPosition(Vector3 newPosition)
        {
            _position = newPosition;
            UpdateVertices();
        }

        public void SetScale(Vector2 newScale)
        {
            _localScale = newScale;
            UpdateVertices();
        }

        public void SetPositionAndScale(Vector3 newPosition, Vector2 newScale)
        {
            _position = newPosition;
            _localScale = newScale;
            UpdateVertices();
        }

        public void SetColor(Color newColor)
        {
            this.color = newColor;
        }

        public void UpdateVertices()
        {
            vertices[0] = new Vector3(_position.x - _localScale.x / 2, _position.y - _localScale.y / 2, _position.z);
            vertices[1] = new Vector3(_position.x - _localScale.x / 2, _position.y + _localScale.y / 2, _position.z);
            vertices[2] = new Vector3(_position.x + _localScale.x / 2, _position.y + _localScale.y / 2, _position.z);
            vertices[3] = new Vector3(_position.x + _localScale.x / 2, _position.y + -_localScale.y / 2, _position.z);
        }
    }
}