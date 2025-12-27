using System.Collections.Generic;
using _Project.Scripts.Core.Bootstrap;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.SharedComponents;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Coin;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Features.EnemySpawner;
using _Project.Scripts.Features.Player.ECS;
using _Project.Scripts.Features.Projectile;
using _Project.Scripts.Infrastructure.View;
using UnityEditor;
using UnityEngine;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Editor
{
    public sealed class EcsEntityListWindow : EditorWindow
    {
        private World _world;
        private ViewRegistry _viewRegistry;
        private Vector2 _scroll;
        private readonly Dictionary<int, bool> _entityFoldouts = new();

        [MenuItem("Tools/ECS/Entity List")]
        public static void Open()
        {
            GetWindow<EcsEntityListWindow>("ECS Entities");
        }

        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            FindWorld();

            if (_world == null)
            {
                EditorGUILayout.HelpBox("World not found", MessageType.Info);
                return;
            }

            DrawEntityList();
        }

        private void FindWorld()
        {
            var entry = FindObjectOfType<GameEntryPoint>();
            if (entry == null)
                return;

            _world = entry.World;
            _viewRegistry = entry.ViewRegistry;
        }

        private void DrawEntityList()
        {
            EditorGUILayout.LabelField("Alive Entities", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            var entities = _world.DebugAliveEntities;

            for (int i = 0; i < entities.Length; i++)
            {
                DrawEntityBlock(entities[i]);
                EditorGUILayout.Space(4);
            }

            EditorGUILayout.EndScrollView();
        }

        private (string label, Color color) GetEntityTagInfo(EntityId entity)
        {
            if (_world.GetPool<PlayerTag>().Has(entity))
                return ("Player", new Color(0.4f, 1f, 0.4f));

            if (_world.GetPool<EnemyTag>().Has(entity))
                return ("Enemy", new Color(1f, 0.4f, 0.4f));

            if (_world.GetPool<ProjectileTag>().Has(entity))
                return ("Projectile", new Color(1f, 1f, 0.4f));

            if (_world.GetPool<CoinTag>().Has(entity))
                return ("Coin", new Color(1f, 0.7f, 0.3f));

            if (_world.GetPool<EnemySpawnerTag>().Has(entity))
                return ("EnemySpawnerTag", new Color(0.5f, 0.4f, 0.4f));

            return ("Unknown", Color.gray);
        }


        private void DrawEntityBlock(EntityId entity)
        {
            int id = entity.Value;

            if (!_entityFoldouts.TryGetValue(id, out bool isOpen))
                isOpen = false;

            var tagInfo = GetEntityTagInfo(entity);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            isOpen = EditorGUILayout.Foldout(isOpen, $"Entity {id}", true);

            var prevColor = GUI.color;
            GUI.color = tagInfo.color;
            GUILayout.Label($"[{tagInfo.label}]", EditorStyles.miniBoldLabel, GUILayout.Width(90));
            GUI.color = prevColor;
            EditorGUILayout.EndHorizontal();
            _entityFoldouts[id] = isOpen;

            if (!isOpen)
            {
                EditorGUILayout.EndVertical();
                return;
            }
            
            EditorGUI.indentLevel++;

            DrawComponents(entity);
            DrawViewInfo(entity);

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void DrawComponents(EntityId entity)
        {
            EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);

            bool hasAny = false;

            foreach (var pool in _world.DebugComponentPools)
            {
                if (!pool.Has(entity))
                    continue;

                hasAny = true;
                EditorGUILayout.LabelField($"• {pool.ComponentType.Name}", EditorStyles.boldLabel);
                DrawComponentValues(pool, entity);
            }

            if (!hasAny)
                EditorGUILayout.LabelField("No components");
        }

        private void DrawComponentValues(IComponentPool pool, EntityId entity)
        {
            var boxed = pool.GetBoxed(entity);
            if (boxed == null)
                return;

            var fields = boxed.GetType().GetFields();

            EditorGUI.indentLevel++;
            foreach (var field in fields)
            {
                var value = field.GetValue(boxed);
                EditorGUILayout.LabelField(
                    field.Name,
                    value != null ? value.ToString() : "null"
                );
            }

            EditorGUI.indentLevel--;
        }
        
        private void DrawViewInfo(EntityId entity)
        {
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("View", EditorStyles.boldLabel);

            if (_viewRegistry == null)
            {
                EditorGUILayout.LabelField("ViewRegistry not found");
                return;
            }

            if (!_viewRegistry.TryGet(entity, out var go) || go == null)
            {
                EditorGUILayout.LabelField("No view");
                return;
            }

            EditorGUI.indentLevel++;

            EditorGUILayout.ObjectField("GameObject", go, typeof(GameObject), true);

            var t = go.transform;

            EditorGUILayout.Vector3Field("Position", t.position);
            EditorGUILayout.Vector3Field("Rotation", t.rotation.eulerAngles);
            EditorGUILayout.Vector3Field("Scale", t.localScale);

            DrawViewStats(entity);

            EditorGUI.indentLevel--;
        }

        private void DrawViewStats(EntityId entity)
        {
            bool hasStats = false;

            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField("UI (ECS)", EditorStyles.miniBoldLabel);
            EditorGUI.indentLevel++;

            if (_world.GetPool<Health>().Has(entity))
            {
                ref var health = ref _world.GetPool<Health>().Get(entity);
                EditorGUILayout.LabelField("HP");
                DrawHpBar(health.Current, health.Max);
                hasStats = true;
            }
            
            if (_world.GetPool<Resource>().Has(entity))
            {
                ref var res = ref _world.GetPool<Resource>().Get(entity);
                EditorGUILayout.LabelField("Coins", res.Value.ToString());
                hasStats = true;
            }

            if (!hasStats)
                EditorGUILayout.LabelField("No UI data");

            EditorGUI.indentLevel--;
        }

        private void DrawHpBar(float current, float max)
        {
            float pct = max > 0 ? current / max : 0f;
            var rect = GUILayoutUtility.GetRect(1, 25);

            EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width * pct, rect.height),
                Color.Lerp(Color.red, Color.green, pct));

            EditorGUI.LabelField(rect, $"{current} / {max}", new GUIStyle(EditorStyles.centeredGreyMiniLabel)
                { normal = { textColor = Color.black } }
            );
        }
    }
}