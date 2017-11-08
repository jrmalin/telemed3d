import numpy as np
import pandas as pd
from sklearn.tree import DecisionTreeClassifier
import warnings


def is_int(x):
  try:
    int(x)
    return True
  except ValueError:
    return False


def parse_features(filename):
  # Read in features from PDF's CSV and remove malformed
  features = pd.read_csv(filename)
  features = features.dropna(subset=['ITEM'])
  features = features[features['ITEM'].apply(lambda x: is_int(x))]

  features = features.rename(columns={
    'ITEM': 'ITEM NO.',
    'FIELD': 'FIELD LENGTH',
    'FILE': 'FILE LOCATION',
    'Unnamed: 3': '[ITEM NAME], DESCRIPTION, AND CODES'
    })

  # Find data errors in features
  for i in range(1, 1067):
    count = len(features[features['ITEM NO.'] == str(i)])
    if count != 1:
      w = str(count) + ' items numbered "' + str(i) + '" instead of 1.'
      warnings.warn(w, Warning)

  # Find data errors in features
  if len(features.isnull().any(1).nonzero()[0]) > 0:
    w = 'nan values at printed indices'
    print(features.isnull().any(1).nonzero())
    warnings.warn(w, Warning)
    print(features.iloc[features.isnull().any(1).nonzero()[0][0]])

  return features


def create_row(features_loc, line):
  new_row = []
  for col_idx, input_idx in enumerate(features_loc):
    if '-' in input_idx:
      split = input_idx.split('-')
      new_row.append(line[int(split[0]) - 1 : int(split[1])])
    else:
      new_row.append(line[int(input_idx) - 1])

  return new_row


def create_df(filename, features):
  features_loc = features['FILE LOCATION']
  features_name = features['[ITEM NAME], DESCRIPTION, AND CODES']
  
  f = open(filename, 'r')
  lines = f.readlines()

  data = []
  for row_idx, line in enumerate(lines):
    new_row = create_row(features_loc, line)
    data.append(new_row)

  df = pd.DataFrame(data, columns=features_name)
  return df
 

def main():
  try:
    print('loading namcs2015 dataframe...')
    df = pd.read_pickle('namcs2015_parsed.pkl')
    print('load complete')
  except:
    print('load failed')
    print('creating namcs2015 dataframe...')
    features = parse_features('tabula_desc.csv')
    df = create_df('namcs2015', features)
    print('creation complete')
    print('saving namcs2015 dataframe...')
    df.to_pickle('namcs2015_parsed.pkl')
    print('save complete')

  X = df[df.columns.difference(['[DIAG1R] DIAGNOSIS # 1 (Recode to Numeric Field)'])] 
  y = df['[DIAG1R] DIAGNOSIS # 1 (Recode to Numeric Field)']
#  clf = DecisionTreeClassifier()
#  clf.fit(X, y)
#  print(clf.score(X, y))
  

if __name__ == "__main__":
  main()



